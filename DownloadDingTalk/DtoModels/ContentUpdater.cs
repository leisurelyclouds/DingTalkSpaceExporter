using Newtonsoft.Json;

namespace DownloadDingTalk.DtoModels
{
    public class ContentUpdater
    {
        [JsonProperty("avatorMediaId", NullValueHandling = NullValueHandling.Ignore)]
        public string AvatorMediaId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }
    }
}
