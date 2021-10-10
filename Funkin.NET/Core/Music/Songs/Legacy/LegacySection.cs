using System.Collections.Generic;
using Newtonsoft.Json;

namespace Funkin.NET.Core.Music.Songs.Legacy
{
    public class LegacySection
    {
        [JsonProperty("mustHitSection")] public bool MustHitSection { get; set; }

        [JsonProperty("typeOfSection")] public int TypeOfSection { get; set; }

        [JsonProperty("lengthInSteps")] public int LengthInSteps { get; set; }

        [JsonProperty("sectionNotes")]
        public List<(double, int, int)> SectionNotes { get; set; }

        [JsonProperty("bpm")] public int? Bpm { get; set; }

        [JsonProperty("changeBPM")] public bool? ChangeBpm { get; set; }
    }
}