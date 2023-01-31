using Newtonsoft.Json;

namespace DownloadDingTalk.DtoModels
{
    public class FluffyDentryStatistic
    {
        [JsonProperty("childrenCount")]
        public long ChildrenCount { get; set; }

        [JsonProperty("previewCount", NullValueHandling = NullValueHandling.Ignore)]
        public long? PreviewCount { get; set; }

        [JsonProperty("downloadCount", NullValueHandling = NullValueHandling.Ignore)]
        public long? DownloadCount { get; set; }
    }
}
