using Newtonsoft.Json;

namespace DownloadDingTalk.DtoModels
{
    public class VisitInfo
    {
        [JsonProperty("lastVisitTime", NullValueHandling = NullValueHandling.Ignore)]
        public long? LastVisitTime { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public long? Type { get; set; }

        [JsonProperty("visited")]
        public bool Visited { get; set; }
    }
}
