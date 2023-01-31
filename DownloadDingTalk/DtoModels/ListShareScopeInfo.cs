using DownloadDingTalk.Models;
using Newtonsoft.Json;

namespace DownloadDingTalk.DtoModels
{
    public class ListShareScopeInfo
    {
        [JsonProperty("scope")]
        public long Scope { get; set; }

        [JsonProperty("roleId", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringConverter))]
        public long? RoleId { get; set; }
    }
}
