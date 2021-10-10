using System.Collections.Generic;
using System.Text.Json.Serialization;
using Funkin.NET.Core.Json;

namespace Funkin.NET.Core.Music.Songs.Legacy
{
    public class LegacySection : ISection
    {
        [JsonPropertyName("mustHitSection")] public bool MustHitSection { get; set; }

        [JsonPropertyName("typeOfSection")] public int TypeOfSection { get; set; }

        [JsonPropertyName("lengthInSteps")] public int LengthInSteps { get; set; }

        [JsonPropertyName("sectionNotes")]
        [JsonConverter(typeof(NoteConverter))]
        public List<LegacyNote> SectionNotes { get; set; }

        [JsonPropertyName("bpm")] public int? Bpm { get; set; }

        [JsonPropertyName("changeBPM")] public bool? ChangeBpm { get; set; }
    }
}