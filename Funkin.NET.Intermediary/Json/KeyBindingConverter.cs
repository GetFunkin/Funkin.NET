using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using osu.Framework.Input.Bindings;

namespace Funkin.NET.Intermediary.Json
{
    /// <summary>
    ///     Custom <see cref="JsonConverter"/> for serializing and deserializing <see cref="IKeyBinding"/>s.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class KeyBindingConverter<T> : JsonConverter<IKeyBinding>
    {
        public override IKeyBinding Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // ensure a start object ('{') exists
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();

            int jsonCycle = 0;
            List<InputKey> keys = new();
            string actionString = "";

            // skipping property names means we only pay attention to their values
            // this ends up resulting in everything being anonymous, so we need to stick
            // to reading things as expected, lest we wish to end up with erroneous deserialization
            while (reader.Read())
            {
                // exit out of the loop after the end object
                if (reader.TokenType == JsonTokenType.EndObject)
                    break;

                // ignore property names
                if (reader.TokenType == JsonTokenType.PropertyName)
                    continue;

                switch (jsonCycle)
                {
                    // at the beginning, get the name of the action
                    case 0:
                        actionString = reader.GetString();
                        break;

                    // if it isn't the beginning, parse the InputKey enum
                    default:
                        keys.Add(Enum.Parse<InputKey>(reader.GetString() ?? ""));
                        break;
                }

                // forward the cycle
                jsonCycle++;
            }

            KeyCombination combo = new(keys);
            object enumAction = Enum.Parse(typeof(T), actionString ?? throw new InvalidOperationException());

            return new KeyBinding(combo, enumAction);
        }

        public override void Write(Utf8JsonWriter writer, IKeyBinding value, JsonSerializerOptions options)
        {
            // {
            writer.WriteStartObject();

            // write the element account to the beginning
            // currently unused, but can be helpful later on
            // was originally used for reading, but we just check for the end object now
            int elemCount = value.KeyCombination.Keys.Length;

            // write the name of the button press
            writer.WriteString("action", value.Action.ToString());

            // write all the keys associated with the action, which we use to map to bindings when deserializing
            for (int i = 0; i < elemCount; i++)
                writer.WriteString($"key{i}", value.KeyCombination.Keys[i].ToString());

            // }
            writer.WriteEndObject();
        }
    }
}