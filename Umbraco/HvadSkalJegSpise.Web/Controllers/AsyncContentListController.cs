using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Umbraco.Web.WebApi;
using HvadSkalJegSpise.Web.Models;
using Umbraco.Web;
using Newtonsoft.Json.Linq;
using Umbraco.Core.Models;
using System.Collections;
using Examine;
using Examine.Providers;
using Examine.SearchCriteria;
using System.Text;
using HvadSkalJegSpise.Web.Helpers;
using System.Globalization;

namespace HvadSkalJegSpise.Web.Controllers
{
    public class AsyncContentListController : UmbracoApiController
    {
        private UmbracoHelper _helper;
        private BaseSearchProvider _ExternalSearcher;
        private AsyncContentListController()
        {
            _helper = new UmbracoHelper(UmbracoContext.Current);
            _ExternalSearcher = ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"];
        }

        [HttpGet]
        public HttpResponseMessage GetListByTags(string tags, string id)
        {
            if(string.IsNullOrEmpty(tags))
                return Request.CreateResponse(HttpStatusCode.Accepted, "");

            var currentPage = _helper.TypedContent(id);
            var tagArray = tags.Split(',');
            var criteria = _ExternalSearcher.CreateSearchCriteria();
            var query = new StringBuilder();
            query.AppendFormat("__IndexType:{0} -umbracoNaviHide:1 +searchPath:{1} ", UmbracoExamine.IndexTypes.Content, currentPage.Id);
            var groupedOr = new StringBuilder();
            foreach (var tag in tagArray)
            {
                groupedOr.AppendFormat("eventTags:{0} ", tag);
            }
            query.Append("+(" + groupedOr.ToString() + ") ");

            var criteria2 = criteria.RawQuery(query.ToString());
            var results = _ExternalSearcher.Search(criteria2);

            var listOfContent = new List<EventContentModel>();
            foreach (var result in results)
            {
                var content = _helper.TypedContent(result.Id);
                var image = content.GetPropertyValue("teaserImage", content.GetPropertyValue("heroImage", Enumerable.Empty<IPublishedContent>()));
                var eventStart = content.GetPropertyValue<DateTime>("eventStart");
                var startTime = content.GetPropertyValue<DateTime>("startTime");
                var eventEnd = content.GetPropertyValue<DateTime>("eventEnd");
                var endTime = content.GetPropertyValue<DateTime>("endTime");
                var tagList = content.GetPropertyValue("eventTags", Enumerable.Empty<IPublishedContent>());

                listOfContent.Add(new EventContentModel {
                    ImageUrl = image.Any() ? image.FirstOrDefault().Url : "",
                    Headline = content.GetPropertyValue<string>("teaserHeadline", content.GetPropertyValue<string>("headline", content.Name)),
                    Lead = content.GetPropertyValue<string>("teaserLead", content.GetPropertyValue<string>("lead", "")),
                    Url = content.Url,
                    StartDate = eventStart.Year > 1 ? eventStart.ToString("d/M yyyy", CultureInfo.InvariantCulture) : "",
                    StartTime = startTime.Year > 1 ? startTime.ToString("d/M yyyy", CultureInfo.InvariantCulture) : "",
                    EndDate = eventEnd.Year > 1 ? eventEnd.ToString("d/M yyyy", CultureInfo.InvariantCulture) : "",
                    Endtime = endTime.Year > 1 ? endTime.ToString("d/M yyyy", CultureInfo.InvariantCulture) : "",
                    EventPlace = content.GetPropertyValue<string>("eventPlace"),
                    Tags = tagList.Any() ? tagList.Select(x => x.Name).ToArray() : new string[0]


                });
            }

            var jObject = JArray.FromObject(
                listOfContent,
                new JsonSerializer { NullValueHandling = NullValueHandling.Ignore }
                );

            return Request.CreateResponse(HttpStatusCode.Accepted, jObject);
        }

        [HttpGet]
        public HttpResponseMessage GetList(string data, int page, int pageSize = 2)
        {
            var dataArray = data.Split(',');
            var list = new List<AsyncContentListModel>();
            if(pageSize > 0)
            {
                list = ContentListById(dataArray)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList();
            } else
            {
                list = ContentListById(dataArray);
            }


            var jObject = JArray.FromObject(
                list,
                new JsonSerializer { NullValueHandling = NullValueHandling.Ignore }
                );

            return Request.CreateResponse(HttpStatusCode.Accepted, jObject);
        }

        private List<AsyncContentListModel> ContentListById(string[] array)
        {
            var list = new List<AsyncContentListModel>();

            foreach (var item in array)
            {
                var typedContent = _helper.TypedContent(item);
                var content = new AsyncContentListModel
                {
                    Name = typedContent.Name,
                    Lead = typedContent.GetPropertyValue<string>("lead", ""),
                    Url = typedContent.Url,
                    Tags = typedContent.GetPropertyValue<IEnumerable<IPublishedContent>>("tags").Select(x => x.Name).ToArray()
                };

                list.Add(content);
            }
            return list;
        }
    }
}
