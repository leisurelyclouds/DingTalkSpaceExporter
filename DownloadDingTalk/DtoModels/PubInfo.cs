using Newtonsoft.Json;

namespace DownloadDingTalk.DtoModels
{
    public class PubInfo
    {
        [JsonProperty("pubUrl")]
        public Uri PubUrl { get; set; }

        [JsonProperty("published")]
        public bool Published { get; set; }
    }
}
