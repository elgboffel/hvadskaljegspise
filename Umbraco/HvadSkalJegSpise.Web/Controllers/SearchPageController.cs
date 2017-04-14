using Examine;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Examine.LuceneEngine.SearchCriteria;

namespace HvadSkalJegSpise.Web.Controllers
{
    public class SearchPageController : RenderMvcController
    {
		//public override ActionResult Index(RenderModel renderModel)
		//{
		//	return Index(renderModel);
		//}

		public ActionResult Index(RenderModel renderModel, string q = "", int p = 1)
		{
			var model = new SearchModel(renderModel.Content, renderModel.CurrentCulture)
			{
				CurrentPage = p,
				SearchTerm = q,
				PageSize = renderModel.Content.GetPropertyValue<int>("pageSize", 100),
				RootContentNode = renderModel.Content.GetPropertyValue<IPublishedContent>("searchStartNode", renderModel.Content.Site()),
				RootMediaNode = renderModel.Content.GetPropertyValue<IPublishedContent>("searchStartMediaNode", null),
				IndexType = "",
				HideFromSearchField = "umbracoSearchHide",
				SearchFields = new List<string> { "nodeName", "headline", "lead", "primaryTags", "secondaryTags", "layout", "importedContent", "teaserHeadline", "teaserLead"},
				PreviewFields = new List<string> { "headline", "lead", "layout", "importedContent", "teaserHeadline", "teaserLead" },
				PreviewLength = 150,
			};

			// If searching on umbracoFile, also search on umbracoFileName
			if (model.SearchFields.Contains("umbracoFile") && !model.SearchFields.Contains("umbracoFileName"))
			{
				model.SearchFields.Add("umbracoFileName");
			}

			// Check the search term isn't empty
			if (!string.IsNullOrWhiteSpace(model.SearchTerm))
			{
				// Tokenize the search term
				model.SearchTerms = Umbraco.Tokenize(model.SearchTerm);

				// Search
				var results = new List<SearchResult>();
				results.AddRange(SearchProvider(model, "ExternalSearcher"));
				//results.AddRange(SearchPDF(model));
				model.AllResults = results.OrderByDescending(item => item.Score).ToList();

				// Count results, and display paged
				model.TotalResults = model.AllResults.Count();
				model.TotalPages = (int)Math.Ceiling((decimal)model.TotalResults / model.PageSize);
				model.CurrentPage = Math.Max(1, Math.Min(model.TotalPages, model.CurrentPage));
				//search.Paged = new PagedResult<SearchResult>(search.AllResults.Count(), search.CurrentPage, search.PageSize);
				// Page the results
				model.PagedResults = model.AllResults.Skip(model.PageSize * (model.CurrentPage - 1)).Take(model.PageSize);
			}

			return View("~/Views/SearchPage.cshtml", model);
		}

		private IEnumerable<SearchResult> SearchProvider(SearchModel model, string provider = "ExternalSearcher")
		{
			var searcher = ExamineManager.Instance.SearchProviderCollection[provider];
			var criteria = searcher.CreateSearchCriteria();
			string query = GetSearchQuery(model);
			var criteria2 = criteria.RawQuery(query);
			var results = searcher.Search(criteria2)
			   .Where(x => (
				   !Umbraco.IsProtected(x.Fields["path"]) ||
				   (
					   Umbraco.IsProtected(x.Fields["path"]) &&
					   Umbraco.MemberHasAccess(x.Fields["path"])
				   )))
				.ToList();

			LogHelper.Info<string>("[Search] Searched " + provider + " found " + results.Count() + " with the following query: " + query.ToString());

			return results;
		}

		private string GetSearchQuery(SearchModel model)
		{
			var query = new StringBuilder();
			query.AppendFormat("-{0}:1 ", model.HideFromSearchField);

			// Set search path
			var contentPathFilter = model.RootContentNode != null
				? string.Format("__IndexType:{0} +searchPath:{1}", UmbracoExamine.IndexTypes.Content, model.RootContentNode.Id)
				: string.Format("__IndexType:{0} -template:0", UmbracoExamine.IndexTypes.Content);

			var mediaPathFilter = model.RootMediaNode != null
				? string.Format("__IndexType:{0} +searchPath:{1}", UmbracoExamine.IndexTypes.Media, model.RootMediaNode.Id)
				: string.Format("__IndexType:{0}", UmbracoExamine.IndexTypes.Media);

			switch (model.IndexType)
			{
				case UmbracoExamine.IndexTypes.Content:
					query.AppendFormat("+({0}) ", contentPathFilter);
					break;
				case UmbracoExamine.IndexTypes.Media:
					query.AppendFormat("+({0}) ", mediaPathFilter);
					break;
				default:
					query.AppendFormat("+(({0}) ({1})) ", contentPathFilter, mediaPathFilter);
					break;
			}

			// Ensure page contains any of the search terms
			foreach (var term in model.SearchTerms)
			{
				var groupedOr = new StringBuilder();
				foreach (var searchField in model.SearchFields)
				{
					groupedOr.AppendFormat("{0}:{1}* ", searchField, term);
				}
				query.Append("+(" + groupedOr.ToString() + ") ");
			}

			// Rank content based on positon of search terms in fields
			for (var i = 0; i < model.SearchFields.Count; i++)
			{
				foreach (var term in model.SearchTerms)
				{
					query.AppendFormat("{0}:{1}*^{2} ", model.SearchFields[i], term, model.SearchFields.Count - i);
				}
			}

			return query.ToString();
		}

		//private ISearchResults SearchPDF(SearchModel model)
		//{
		//	var searcher = ExamineManager.Instance.SearchProviderCollection["PDFSearcher"];
		//	string[] fields = new string[] { "FileTextContent", "nodeName" };
		//	var terms = model.SearchTerms.Select(x => x.MultipleCharacterWildcard()).ToArray();
		//	var criteria = searcher.CreateSearchCriteria();

		//	var query = criteria.GroupedOr(fields, terms).Compile();

		//	if (model.RootMediaNode != null)
		//	{
		//		query.Field("searchPath", model.RootMediaNode.Id.ToString()).Compile();
		//	}

		//	LogHelper.Info<SearchPageController>("PDFSearcher: {0}", () => query.ToString());

		//	return searcher.Search(query);
		//}
	}

	public class SearchModel : RenderModel
	{
		public SearchModel(IPublishedContent content, CultureInfo culture)
		  : base(content, culture)
		{
			this.AllResults = new List<SearchResult>();
			this.PagedResults = Enumerable.Empty<SearchResult>();
		}

		// Query Parameters
		public string SearchTerm { get; set; }
		public IEnumerable<string> SearchTerms { get; set; }
		public int CurrentPage { get; set; }

		// Options
		public IPublishedContent RootContentNode { get; set; }
		public IPublishedContent RootMediaNode { get; set; }
		public IList<string> SearchFields { get; set; }
		public IList<string> PreviewFields { get; set; }
		public IList<string> FileExtensions { get; set; }
		public int PreviewLength { get; set; }
		public int PageSize { get; set; }
		public string HideFromSearchField { get; set; }
		public string IndexType { get; set; }

		// Results
		public int TotalResults { get; set; }
		public int TotalPages { get; set; }

		public List<SearchResult> AllResults { get; set; }
		public IEnumerable<SearchResult> PagedResults { get; set; }
	}
}
