using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;

namespace Funkin.NET.Songs
{
    public class Song
    {
        [JsonPropertyName("player2")]
        public string Player2 { get; set; }

        [JsonPropertyName("player1")]
        public string Player1 { get; set; }

        [JsonPropertyName("speed")]
        public double Speed { get; set; }

        [JsonPropertyName("needsVoices")]
        public bool NeedsVoices { get; set; }

        [JsonPropertyName("sectionLengths")]
        public List<object> SectionLengths { get; set; }

        [JsonPropertyName("song")]
        public string SongName { get; set; }

        [JsonPropertyName("notes")]
        public List<Section> Sections { get; set; }

        [JsonPropertyName("bpm")]
        public int Bpm { get; set; }

        [JsonPropertyName("sections")]
        public int NumSections { get; set; }

        public static Song GetSong(string json)
        {
            RootTrack track = RootTrack.GetTrack(json);
            return track.Song;
        }

        public static Song GetSongFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new ArgumentException("File doesn't exist", nameof(filePath));

            string json = File.ReadAllText(filePath);
            return RootTrack.GetTrack(json).Song;
        }
    }
}