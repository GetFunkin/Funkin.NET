using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Funkin.NET.Core.Music.Songs.Legacy
{
    public class LegacyTrack
    {
        [JsonProperty("song")]
        public virtual LegacySong Song { get; set; }

        [JsonProperty("bpm")] public virtual double Bpm { get; set; }

        [JsonProperty("sections")] public virtual int Sections { get; set; }
        
        [JsonProperty("notes")]
        public virtual List<LegacySection> Notes { get; set; }

        public static LegacyTrack GetTrack(string json)
        {
            LegacyTrack track = JsonConvert.DeserializeObject<LegacyTrack>(json);
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