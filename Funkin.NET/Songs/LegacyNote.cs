using Newtonsoft.Json;

namespace Funkin.NET.Songs
{
    /// <summary>
    ///     Implementation of the regular FNF note format.
    /// </summary>
    public class LegacyNote : INote
    {
        [JsonProperty("sectionNotes")] public object[] SectionNotes { get; set; }

        [JsonProperty("lengthInSteps")] public int LengthInSteps { get; set; }

        [JsonProperty("typeOfSection")] public int TypeOfSection { get; set; }

        [JsonProperty("mustHitSection")] public bool MustHitSection { get; set; }

        [JsonProperty("bpm")] public double Bpm { get; set; } = -1D;

        [JsonProperty("changeBpm")] public bool ChangeBpm { get; set; }

        [JsonProperty("altAnim")] public bool AltAnim { get; set; }

        public LegacyNote(object[] sectionNotes, int lengthInSteps, int typeOfSection, bool mustHitSection)
        {
            SectionNotes = sectionNotes;
            LengthInSteps = lengthInSteps;
            TypeOfSection = typeOfSection;
            MustHitSection = mustHitSection;
        }
    }
}