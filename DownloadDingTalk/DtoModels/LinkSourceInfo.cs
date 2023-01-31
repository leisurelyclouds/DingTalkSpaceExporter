using DownloadDingTalk.Models;
using Newtonsoft.Json;

namespace DownloadDingTalk.DtoModels
{
    public class LinkSourceInfo
    {
        [JsonProperty("iconUrl")]
        public Url IconUrl { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("linkType")]
        public long LinkType { get; set; }

        [JsonProperty("spaceId")]
        public string SpaceId { get; set; }
    }
}
