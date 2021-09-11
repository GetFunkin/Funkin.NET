﻿using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Serialization;
using Funkin.NET.Resources;

namespace Funkin.NET.Core.Music.Songs
{
    public class Song
    {
        [JsonPropertyName("player2")] public string Player2 { get; set; }

        [JsonPropertyName("player1")] public string Player1 { get; set; }

        [JsonPropertyName("speed")] public double Speed { get; set; }

        [JsonPropertyName("needsVoices")] public bool NeedsVoices { get; set; }

        [JsonPropertyName("sectionLengths")] public List<object> SectionLengths { get; set; }

        [JsonPropertyName("song")] public string SongName { get; set; }

        [JsonPropertyName("notes")] public List<Section> Sections { get; set; }

        [JsonPropertyName("bpm")] public int Bpm { get; set; }

        [JsonPropertyName("sections")] public int NumSections { get; set; }

        public static Song GetSong(string json)
        {
            RootTrack track = RootTrack.GetTrack(json);
            return track.Song;
        }

        public static Song GetSongFromFile(string filePath, Assembly assembly = null) => GetSong(PathHelper.Json.GetEmbeddedJson(
                PathHelper.EmbeddedResource.SanitizeForEmbeds(filePath, assembly), assembly));
    }
}