using Newtonsoft.Json;
using System;

namespace Bhbk.Lib.DataState.Converters
{
    public class ModelConverter<T, Tt> : JsonConverter where T : Tt
    {
        public override bool CanConvert(Type objectType) =>
            (objectType == typeof(Tt));

        public override object ReadJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) =>
                serializer.Deserialize<T>(reader);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) =>
            serializer.Serialize(writer, value, typeof(T));
    }
}
