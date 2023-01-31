using Newtonsoft.Json;

namespace DownloadDingTalk.DtoModels
{

    internal class ContentTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ContentType) || t == typeof(ContentType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "alidoc":
                    return ContentType.Alidoc;
                case "document":
                    return ContentType.Document;
                case "link":
                    return ContentType.Link;
            }
            throw new Exception("Cannot unmarshal type ContentType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (ContentType)untypedValue;
            switch (value)
            {
                case ContentType.Alidoc:
                    serializer.Serialize(writer, "alidoc");
                    return;
                case ContentType.Document:
                    serializer.Serialize(writer, "document");
                    return;
                case ContentType.Link:
                    serializer.Serialize(writer, "link");
                    return;
            }
            throw new Exception("Cannot marshal type ContentType");
        }

        public static readonly ContentTypeConverter Singleton = new ContentTypeConverter();
    }
}
