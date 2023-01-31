using Newtonsoft.Json;

namespace DownloadDingTalk.DtoModels
{
    public class PurpleDentryStatistic
    {
        [JsonProperty("childrenCount")]
        public long ChildrenCount { get; set; }
    }
}
