using Newtonsoft.Json;

namespace DownloadDingTalk.Models
{
    public class ForbiddenCopy
    {
        [JsonProperty("isInherit")]
        public bool IsInherit { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public bool Value { get; set; }
    }
}
