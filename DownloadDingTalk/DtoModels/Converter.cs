using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace DownloadDingTalk.DtoModels
{
    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                ContentTypeConverter.Singleton,
                DentryTypeConverter.Singleton,
                ExtensionConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
