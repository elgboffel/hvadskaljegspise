using Newtonsoft.Json;
using System;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace HvadSkalJegSpise.Web.Models
{
    public class EventContentModel
    {
        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("headline")]
        public string Headline { get; set; }

        [JsonProperty("lead")]
        public string Lead { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("tags")]
        public string[] Tags { get; set; }

        [JsonProperty("startDate")]
        public string StartDate { get; set; }

        [JsonProperty("endDate")]
        public string EndDate { get; set; }

        [JsonProperty("startTime")]
        public string StartTime { get; set; }

        [JsonProperty("endtime")]
        public string Endtime { get; set; }

        [JsonProperty("eventPlace")]
        public string EventPlace { get; set; }
    }
}

