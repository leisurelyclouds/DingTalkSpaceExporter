using Newtonsoft.Json;

namespace DownloadDingTalk.Models
{
    public class SpaceProfile
    {
        [JsonProperty("createdTime")]
        public long CreatedTime { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("driveSpaceId")]
        public string DriveSpaceId { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public long Type { get; set; }

        [JsonProperty("updatedTime")]
        public long UpdatedTime { get; set; }
    }
}
