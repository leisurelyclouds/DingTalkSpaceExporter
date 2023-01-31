using DownloadDingTalk.Models;
using Newtonsoft.Json;

namespace DownloadDingTalk.DtoModels
{
    public class Pinned
    {
        [JsonProperty("contentUpdatedTime", NullValueHandling = NullValueHandling.Ignore)]
        public long? ContentUpdatedTime { get; set; }

        [JsonProperty("contentUpdater", NullValueHandling = NullValueHandling.Ignore)]
        public ContentUpdater ContentUpdater { get; set; }

        [JsonProperty("corpId")]
        public string CorpId { get; set; }

        [JsonProperty("cover")]
        public Uri Cover { get; set; }

        [JsonProperty("createdTime")]
        public long CreatedTime { get; set; }

        [JsonProperty("creator")]
        public ContentUpdater Creator { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("driveDentryId")]
        public string DriveDentryId { get; set; }

        [JsonProperty("driveSpaceId")]
        public string DriveSpaceId { get; set; }

        [JsonProperty("extraAttributeList")]
        public object[] ExtraAttributeList { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("iconStruct", NullValueHandling = NullValueHandling.Ignore)]
        public string IconStruct { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("openCustomHomepage", NullValueHandling = NullValueHandling.Ignore)]
        public bool? OpenCustomHomepage { get; set; }

        [JsonProperty("orgId")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long OrgId { get; set; }

        [JsonProperty("owner")]
        public ContentUpdater Owner { get; set; }

        [JsonProperty("recentList")]
        public PinnedRecentList[] RecentList { get; set; }

        [JsonProperty("securityInfo")]
        public SecurityInfo SecurityInfo { get; set; }

        [JsonProperty("shareScopeInfo")]
        public PinnedShareScopeInfo ShareScopeInfo { get; set; }

        [JsonProperty("spaceUuid")]
        public string SpaceUuid { get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("subType")]
        public long SubType { get; set; }

        [JsonProperty("type")]
        public long Type { get; set; }

        [JsonProperty("updatedTime")]
        public long UpdatedTime { get; set; }

        [JsonProperty("updator")]
        public ContentUpdater Updator { get; set; }

        [JsonProperty("usedQuota")]
        public long UsedQuota { get; set; }

        [JsonProperty("visitorRelatedInfo")]
        public VisitorRelatedInfo VisitorRelatedInfo { get; set; }

        [JsonProperty("catalogueType", NullValueHandling = NullValueHandling.Ignore)]
        public long? CatalogueType { get; set; }

        [JsonProperty("dynamicColumnType", NullValueHandling = NullValueHandling.Ignore)]
        public long? DynamicColumnType { get; set; }

        [JsonProperty("catalogueConfig", NullValueHandling = NullValueHandling.Ignore)]
        public string CatalogueConfig { get; set; }
    }
}
