﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Funkin.NET.Core.Models
{
    public class Note
    {
        [JsonPropertyName("mustHitSection")]
        public bool MustHitSection { get; set; }

        [JsonPropertyName("typeOfSection")]
        public int TypeOfSection { get; set; }

        [JsonPropertyName("lengthInSteps")]
        public int LengthInSteps { get; set; }

        [JsonPropertyName("sectionNotes")]
        public List<List<int>> SectionNotes { get; set; }

        [JsonPropertyName("bpm")]
        public int? Bpm { get; set; }

        [JsonPropertyName("changeBPM")]
        public bool? ChangeBPM { get; set; }
    }
}