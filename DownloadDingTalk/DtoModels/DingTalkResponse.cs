using Newtonsoft.Json;

namespace DownloadDingTalk.Models
{
    /// <summary>
    /// 目录项
    /// </summary>
    public class DingTalkResponse<T>
    {
        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
