using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Funkin.NET.Common.Input;
using Funkin.NET.Core.Music.Songs;

namespace Funkin.NET.Core.Json
{
    public class NoteConverter : JsonConverter<List<Note>>
    {
        public override List<Note> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();

            using JsonDocument array = JsonDocument.ParseValue(ref reader);

            return (from note in array.RootElement.EnumerateArray()
                select note.EnumerateArray().ToArray()
                into noteArray
                let offset = noteArray[0].GetInt32()
                let key = noteArray[1].GetInt32()
                let holdTime = noteArray[2].GetInt32()
                select new Note(offset, (KeyAction) key, holdTime)).ToList();
        }

        public override void Write(Utf8JsonWriter writer, List<Note> value, JsonSerializerOptions options)
        {
            // TODO: Implement serialization of Note
            throw new NotImplementedException();
        }
    }
}