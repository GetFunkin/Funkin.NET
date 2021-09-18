using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Funkin.NET.Intermediary.Json
{
    public class InterfaceConverter<TImplementer, TInterface> : JsonConverter<TInterface>
        where TImplementer : TInterface
    {
        public override TInterface Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            JsonSerializer.Deserialize<TImplementer>(ref reader, options);

        public override void Write(Utf8JsonWriter writer, TInterface value, JsonSerializerOptions options) =>
            JsonSerializer.Serialize(value, typeof(TImplementer), options);
    }
}