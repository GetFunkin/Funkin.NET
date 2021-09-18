using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Funkin.NET.Intermediary.Json
{
    public class ListInterfaceConverter<TImplementer, TInterface> : JsonConverter<List<TInterface>>
        where TImplementer : TInterface
    {
        public override List<TInterface> Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            List<TImplementer> list = JsonSerializer.Deserialize<List<TImplementer>>(ref reader, options);

            return list?.Select(x => (TInterface) x).ToList();
        }

        public override void Write(Utf8JsonWriter writer, List<TInterface> value, JsonSerializerOptions options) =>
            JsonSerializer.Serialize(value, typeof(List<TImplementer>), options);
    }
}