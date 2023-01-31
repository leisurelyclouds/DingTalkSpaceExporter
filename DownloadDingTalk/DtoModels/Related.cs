using Newtonsoft.Json;

namespace DownloadDingTalk.DtoModels
{
    public class Related
    {
        [JsonProperty("hasMore")]
        public bool HasMore { get; set; }

        [JsonProperty("list")]
        public List[] List { get; set; }

        [JsonProperty("loadMoreId")]
        public string LoadMoreId { get; set; }
    }
}
