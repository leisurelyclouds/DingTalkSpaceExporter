using Newtonsoft.Json;

namespace DownloadDingTalk.DtoModels
{
    public class Hidden
    {
        [JsonProperty("totalCount")]
        public long TotalCount { get; set; }
    }
}
