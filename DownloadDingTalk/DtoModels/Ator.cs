using Newtonsoft.Json;

namespace DownloadDingTalk.Models
{
    public class Ator
    {
        [JsonProperty("avatorMediaId")]
        public string AvatorMediaId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("uid")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Uid { get; set; }
    }
}
