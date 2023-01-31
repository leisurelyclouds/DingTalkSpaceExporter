using Newtonsoft.Json;

namespace DownloadDingTalk.DtoModels
{
    internal class DentryTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(DentryType) || t == typeof(DentryType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "file":
                    return DentryType.File;
                case "folder":
                    return DentryType.Folder;
            }
            throw new Exception("Cannot unmarshal type DentryType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (DentryType)untypedValue;
            switch (value)
            {
                case DentryType.File:
                    serializer.Serialize(writer, "file");
                    return;
                case DentryType.Folder:
                    serializer.Serialize(writer, "folder");
                    return;
            }
            throw new Exception("Cannot marshal type DentryType");
        }

        public static readonly DentryTypeConverter Singleton = new DentryTypeConverter();
    }
}
