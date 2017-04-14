using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Umbraco.Web;

namespace HvadSkalJegSpise.Web.Helpers
{
	public class SearchHelper
	{
		private UmbracoHelper umbracoHelper;

		#region Constructors
		public SearchHelper(UmbracoContext context)
		{
			umbracoHelper = new UmbracoHelper(context);
		}
		#endregion

		// Cleanse the search term
		public string CleanseSearchTerm(string input)
		{
			return umbracoHelper.StripHtml(input).ToString();
		}

		// Splits a string on space, except where enclosed in quotes
		public IEnumerable<string> Tokenize(string input)
		{
			return Regex.Matches(input, @"[\""].+?[\""]|[^ ]+")
				.Cast<Match>()
				.Select(m => m.Value.Trim('\"'))
				.ToList();
		}

		// Highlights all occurances of the search terms in a body of text
		public IHtmlString Highlight(IHtmlString input, IEnumerable<string> searchTerms)
		{
			return Highlight(input.ToString(), searchTerms);
		}

		// Highlights all occurances of the search terms in a body of text
		public IHtmlString Highlight(string input, IEnumerable<string> searchTerms)
		{
			input = HttpUtility.HtmlDecode(input);

			foreach (var searchTerm in searchTerms)
			{
				input = Regex.Replace(input,
					Regex.Escape(searchTerm),
					"<strong class=\"highlight\">$0</strong>",
					 RegexOptions.IgnoreCase);
			}

			return new HtmlString(input);
		}

		public IHtmlString FirstParagraphContain(IHtmlString input, int maxLength, IEnumerable<string> searchTerms)
		{
			return FirstParagraphContain(input.ToString(), maxLength, searchTerms);
		}

		/// <summary>
		/// Finds the first occurrence of search terms in the text.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="maxLength"></param>
		/// <param name="searchTerms"></param>
		/// <returns></returns>
		public IHtmlString FirstParagraphContain(string input, int maxLength, IEnumerable<string> searchTerms)
		{

			// https://regex101.com/r/hP6gS1/16
			string terms = string.Join("|", searchTerms);
			Regex rg = new Regex(@"([^.?!]*[^.?!](" + terms + @")[^.?!]*[.?!])",
				RegexOptions.Multiline | RegexOptions.IgnoreCase);

			string paragraph = input;

			Match match = rg.Match(paragraph);
			if (match.Success)
			{
				paragraph = input.Substring(match.Groups[0].Index);
			}

			string truncated = Truncate(paragraph, maxLength);
			IHtmlString highlighted = Highlight(truncated, searchTerms);

			return highlighted;
		}

		// Truncates a string on word breaks
		public string Truncate(IHtmlString input, int maxLength)
		{
			return Truncate(input.ToString(), maxLength);
		}

		// Truncates a string on word breaks
		public string Truncate(string input, int maxLength)
		{
			var truncated = umbracoHelper.Truncate(input, maxLength, true).ToString();
			if (truncated.EndsWith("&hellip;"))
			{
				var lastSpaceIndex = truncated.LastIndexOf(' ');
				if (lastSpaceIndex > 0)
				{
					truncated = truncated.Substring(0, lastSpaceIndex) + "&hellip;";
				}
			}

			return truncated;
		}

		// Splits a coma seperated string into a list
		public IList<string> SplitToList(string input)
		{
			return input.Split(',')
				.Select(f => f.Trim())
				.Where(f => !string.IsNullOrEmpty(f))
				.ToList();
		}
	}
}
