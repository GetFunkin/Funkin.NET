using System.Collections.Generic;

namespace Funkin.NET.Core.Music.Songs.Legacy
{
    public interface ISection
    {
        bool MustHitSection { get; }

        int TypeOfSection { get; }

        int LengthInSteps { get; }

        List<LegacyNote> SectionNotes { get; }

        int? Bpm { get; }

        bool? ChangeBpm { get; }
    }
}