using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Funkin.NET.Input.Bindings;

namespace Funkin.NET.Songs
{
    public class NoteConverter : JsonConverter<List<Note>>
    {
        public override List<Note> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();

            using JsonDocument array = JsonDocument.ParseValue(ref reader);

            List<Note> notes = new();
            foreach (JsonElement note in array.RootElement.EnumerateArray())
            {
                JsonElement[] noteArray = note.EnumerateArray().ToArray();
                int offset = noteArray[0].GetInt32();
                int key = noteArray[1].GetInt32();
                int holdTime = noteArray[2].GetInt32();
                notes.Add(new Note(offset, (UniversalAction) key, holdTime));
            }

            return notes;
        }

        public override void Write(Utf8JsonWriter writer, List<Note> value, JsonSerializerOptions options)
        {
            // TODO: Implement serialization of Note
            throw new NotImplementedException();
        }
    }
}