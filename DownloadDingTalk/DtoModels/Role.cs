using Newtonsoft.Json;

namespace DownloadDingTalk.Models
{
    public class Role
    {
        [JsonProperty("code")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Code { get; set; }
    }
}
