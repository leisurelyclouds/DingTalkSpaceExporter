using Newtonsoft.Json;

namespace DownloadDingTalk.DtoModels
{
    public class SpaceData
    {
        [JsonProperty("hidden")]
        public Hidden Hidden { get; set; }

        [JsonProperty("pinned")]
        public Pinned[] Pinned { get; set; }

        [JsonProperty("related")]
        public Related Related { get; set; }

        [JsonProperty("totalCount")]
        public long TotalCount { get; set; }
    }
}
