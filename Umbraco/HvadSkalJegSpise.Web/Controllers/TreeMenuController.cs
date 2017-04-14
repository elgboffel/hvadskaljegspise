using System.Web.Http;
using Umbraco.Web.Mvc;
using Umbraco.Core.Services;
using System.Linq;
using Umbraco.Web;
using Umbraco.Web.WebApi;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using TreeMenu.Models;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TreeMenu.Controllers
{
    public class TreeMenuController : UmbracoApiController
    {
        [HttpGet]
        public HttpResponseMessage GetTreeLevel(int id)
        {
            if(id <= 0)
                return Request.CreateValidationErrorResponse("Invalid parameter id");

            var context = Umbraco.TypedContent(id);

            var children = context.Children().Select(x=>new TreeMenuModelItem(x)).ToList();

            var result = new TreeMenuModel {
                   Children = children,
                   Parent = new TreeMenuModelItem(context)
            };

            var json = JsonConvert.SerializeObject(
                result,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

            return Request.CreateResponse(HttpStatusCode.Accepted, json);
        }
    }
}
