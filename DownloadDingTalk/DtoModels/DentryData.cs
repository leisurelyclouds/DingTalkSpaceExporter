using DownloadDingTalk.DtoModels;
using Newtonsoft.Json;

namespace DownloadDingTalk.Models
{
    public class DentryData
    {
        [JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        public DentryData[] Children { get; set; }

        [JsonProperty("createdTime")]
        public long CreatedTime { get; set; }

        [JsonProperty("creator")]
        public Ator Creator { get; set; }

        [JsonProperty("dentryId")]
        public string DentryId { get; set; }

        [JsonProperty("dentryKey")]
        public string DentryKey { get; set; }

        [JsonProperty("dentryStatistic")]
        public DentryStatistic DentryStatistic { get; set; }

        [JsonProperty("dentryType")]
        public DentryType DentryType { get; set; }

        [JsonProperty("depth")]
        public long Depth { get; set; }

        [JsonProperty("driveDentryId")]
        public string DriveDentryId { get; set; }

        [JsonProperty("driveSpaceId")]
        public string DriveSpaceId { get; set; }

        [JsonProperty("hasChildren")]
        public bool HasChildren { get; set; }

        [JsonProperty("hasMore", NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasMore { get; set; }

        [JsonProperty("illegal")]
        public bool Illegal { get; set; }

        [JsonProperty("isInTrash")]
        public bool IsInTrash { get; set; }

        [JsonProperty("loadMoreId", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringConverter))]
        public long? LoadMoreId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("positionCursor")]
        public string PositionCursor { get; set; }

        [JsonProperty("root", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Root { get; set; }

        [JsonProperty("securityInfo")]
        public SecurityInfo SecurityInfo { get; set; }

        [JsonProperty("spaceId")]
        public string SpaceId { get; set; }

        [JsonProperty("spaceProfile")]
        public SpaceProfile SpaceProfile { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("updatedTime")]
        public long UpdatedTime { get; set; }

        [JsonProperty("updator")]
        public Ator Updator { get; set; }

        [JsonProperty("url")]
        public Url Url { get; set; }

        [JsonProperty("visitorRelatedInfo")]
        public VisitorRelatedInfo VisitorRelatedInfo { get; set; }

        [JsonProperty("ancestorList", NullValueHandling = NullValueHandling.Ignore)]
        public object[] AncestorList { get; set; }

        [JsonProperty("dentryUuid", NullValueHandling = NullValueHandling.Ignore)]
        public string DentryUuid { get; set; }

        [JsonProperty("parentDentryId", NullValueHandling = NullValueHandling.Ignore)]
        public string ParentDentryId { get; set; }
    }
}
