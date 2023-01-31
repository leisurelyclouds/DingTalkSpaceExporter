using Newtonsoft.Json;

namespace DownloadDingTalk.Models
{
    public class SecurityInfo
    {
        [JsonProperty("forbiddenCopy")]
        public ForbiddenCopy ForbiddenCopy { get; set; }

        [JsonProperty("forbiddenSpread")]
        public ForbiddenCopy ForbiddenSpread { get; set; }

        [JsonProperty("watermark")]
        public ForbiddenCopy Watermark { get; set; }
    }
}
