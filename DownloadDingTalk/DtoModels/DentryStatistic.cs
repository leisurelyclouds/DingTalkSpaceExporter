using Newtonsoft.Json;

namespace DownloadDingTalk.Models
{
    public class DentryStatistic
    {
        [JsonProperty("childrenCount")]
        public long ChildrenCount { get; set; }
    }
}
