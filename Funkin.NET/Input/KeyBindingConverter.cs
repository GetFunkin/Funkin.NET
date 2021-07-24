using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using osu.Framework.Input.Bindings;

namespace Funkin.NET.Input
{
    public class KeyBindingConverter<T> : JsonConverter<IKeyBinding>
    {
        public override IKeyBinding Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();

            int jsonCycle = 0;
            List<InputKey> keys = new();
            string actionString = "";

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    break;

                if (reader.TokenType == JsonTokenType.PropertyName)
                    continue;

                switch (jsonCycle)
                {
                    case 0:
                        actionString = reader.GetString();
                        break;

                    default:
                        keys.Add(Enum.Parse<InputKey>(reader.GetString() ?? ""));
                        break;
                }

                jsonCycle++;
            }

            return new KeyBinding(new KeyCombination(keys),
                Enum.Parse(typeof(T), actionString ?? throw new InvalidOperationException()));
        }

        public override void Write(Utf8JsonWriter writer, IKeyBinding value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            int elemCount = value.KeyCombination.Keys.Length;

            writer.WriteString("action", value.Action.ToString());

            for (int i = 0; i < elemCount; i++)
                writer.WriteString($"key{i}", value.KeyCombination.Keys[i].ToString());

            writer.WriteEndObject();
        }
    }
}