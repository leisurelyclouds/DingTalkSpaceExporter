using DownloadDingTalk.Models;
using Newtonsoft.Json;

namespace DownloadDingTalk.DtoModels
{
    public class VisitorRelatedInfo
    {
        [JsonProperty("dentryActions")]
        public string[] DentryActions { get; set; }

        [JsonProperty("hasForbidden")]
        public bool HasForbidden { get; set; }

        [JsonProperty("pinned")]
        public bool Pinned { get; set; }

        [JsonProperty("relatedMarks")]
        public object[] RelatedMarks { get; set; }

        [JsonProperty("roleId")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long RoleId { get; set; }

        [JsonProperty("roles")]
        public Role[] Roles { get; set; }

        [JsonProperty("spaceActions")]
        public string[] SpaceActions { get; set; }

        [JsonProperty("visitInfo")]
        public VisitInfo VisitInfo { get; set; }
    }
}
