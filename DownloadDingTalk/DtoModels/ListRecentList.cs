using DownloadDingTalk.Models;
using Newtonsoft.Json;

namespace DownloadDingTalk.DtoModels
{
    public class ListRecentList
    {
        [JsonProperty("createdTime")]
        public long CreatedTime { get; set; }

        [JsonProperty("creator")]
        public ContentUpdater Creator { get; set; }

        [JsonProperty("dentryId")]
        public string DentryId { get; set; }

        [JsonProperty("dentryKey")]
        public string DentryKey { get; set; }

        [JsonProperty("dentryStatistic")]
        public FluffyDentryStatistic DentryStatistic { get; set; }

        [JsonProperty("dentryType")]
        [JsonConverter(typeof(DentryTypeConverter))]
        public DentryType DentryType { get; set; }

        [JsonProperty("dentryUuid", NullValueHandling = NullValueHandling.Ignore)]
        public string DentryUuid { get; set; }

        [JsonProperty("depth")]
        public long Depth { get; set; }

        [JsonProperty("driveDentryId")]
        public string DriveDentryId { get; set; }

        [JsonProperty("driveSpaceId")]
        public string DriveSpaceId { get; set; }

        [JsonProperty("hasChildren")]
        public bool HasChildren { get; set; }

        [JsonProperty("illegal")]
        public bool Illegal { get; set; }

        [JsonProperty("isInTrash")]
        public bool IsInTrash { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("parentDentryId")]
        public string ParentDentryId { get; set; }

        [JsonProperty("positionCursor")]
        public string PositionCursor { get; set; }

        [JsonProperty("spaceId")]
        public string SpaceId { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("updatedTime")]
        public long UpdatedTime { get; set; }

        [JsonProperty("updator")]
        public ContentUpdater Updator { get; set; }

        [JsonProperty("url")]
        public Url Url { get; set; }

        [JsonProperty("contentType", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ContentTypeConverter))]
        public ContentType? ContentType { get; set; }

        [JsonProperty("docKey", NullValueHandling = NullValueHandling.Ignore)]
        public string DocKey { get; set; }

        [JsonProperty("extension", NullValueHandling = NullValueHandling.Ignore)]
        public string Extension { get; set; }

        [JsonProperty("protocolVersion", NullValueHandling = NullValueHandling.Ignore)]
        public long? ProtocolVersion { get; set; }

        [JsonProperty("linkSourceInfo", NullValueHandling = NullValueHandling.Ignore)]
        public LinkSourceInfo LinkSourceInfo { get; set; }

        [JsonProperty("fileSize", NullValueHandling = NullValueHandling.Ignore)]
        public long? FileSize { get; set; }
    }
}
