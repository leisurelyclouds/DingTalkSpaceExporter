using Newtonsoft.Json;

namespace DownloadDingTalk.DtoModels
{
    internal class ExtensionConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Extension) || t == typeof(Extension?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "adoc":
                    return Extension.Adoc;
                case "amind":
                    return Extension.Amind;
                case "axls":
                    return Extension.Axls;
            }
            throw new Exception("Cannot unmarshal type Extension");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Extension)untypedValue;
            switch (value)
            {
                case Extension.Adoc:
                    serializer.Serialize(writer, "adoc");
                    return;
                case Extension.Amind:
                    serializer.Serialize(writer, "amind");
                    return;
                case Extension.Axls:
                    serializer.Serialize(writer, "axls");
                    return;
            }
            throw new Exception("Cannot marshal type Extension");
        }

        public static readonly ExtensionConverter Singleton = new ExtensionConverter();
    }
}
