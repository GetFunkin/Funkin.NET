﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Funkin.NET.Core.Models
{
    public class RootTrack
    {
        [JsonPropertyName("song")]
        public Song Song { get; set; }

        [JsonPropertyName("bpm")]
        public int Bpm { get; set; }

        [JsonPropertyName("sections")]
        public int Sections { get; set; }

        [JsonPropertyName("notes")]
        public List<Note> Notes { get; set; }

        public static RootTrack GetTrack(string json)
        {
            RootTrack track = JsonSerializer.Deserialize<RootTrack>(json);
            return track;
        }

        public static RootTrack GetTrackFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new ArgumentException("File doesn't exist", nameof(filePath));

            string json = File.ReadAllText(filePath);
            return GetTrack(json);
        }
    }
}