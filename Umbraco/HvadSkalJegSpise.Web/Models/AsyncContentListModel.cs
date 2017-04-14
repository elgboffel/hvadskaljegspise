using Newtonsoft.Json;

namespace HvadSkalJegSpise.Web.Models
{
    public class AsyncContentListModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("lead")]
        public string Lead { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("tags")]
        public string[] Tags { get; set; }
    }
}
