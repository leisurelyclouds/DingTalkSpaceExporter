using Newtonsoft.Json;

namespace DownloadDingTalk.DtoModels
{
    public class PinnedShareScopeInfo
    {
        [JsonProperty("scope")]
        public long Scope { get; set; }
    }
}
