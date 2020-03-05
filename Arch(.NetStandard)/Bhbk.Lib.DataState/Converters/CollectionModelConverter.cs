using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bhbk.Lib.DataState.Converters
{
    public class CollectionModelConverter<T, Tt> : JsonConverter where T : Tt
    {
        public override bool CanConvert(Type objectType) =>
            objectType == typeof(ICollection<Tt>);

        public override object ReadJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) =>
                serializer.Deserialize<ICollection<T>>(reader)?.Cast<Tt>()?.ToList();

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) =>
            serializer.Serialize(writer, value, typeof(ICollection<T>));
    }
}
