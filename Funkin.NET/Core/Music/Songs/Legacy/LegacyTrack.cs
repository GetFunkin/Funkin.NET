using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Funkin.NET.Intermediary.Json;

namespace Funkin.NET.Core.Music.Songs.Legacy
{
    public class LegacyTrack : ITrack
    {
        [JsonConverter(typeof(InterfaceConverter<LegacySong, ISong>))]
        [JsonPropertyName("song")]
        public virtual ISong Song { get; set; }

        [JsonPropertyName("bpm")] public virtual double Bpm { get; set; }

        [JsonPropertyName("sections")] public virtual int Sections { get; set; }

        [JsonConverter(typeof(ListInterfaceConverter<LegacySection, ISection>))]
        [JsonPropertyName("notes")]
        public virtual List<ISection> Notes { get; set; }

        public static LegacyTrack GetTrack(string json)
        {
            LegacyTrack track = JsonSerializer.Deserialize<LegacyTrack>(json);
            return track;
        }

        public static LegacyTrack GetTrackFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new ArgumentException("File doesn't exist", nameof(filePath));

            return GetTrack(File.ReadAllText(filePath));
        }
    }
}