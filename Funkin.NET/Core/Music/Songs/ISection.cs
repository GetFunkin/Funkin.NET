using System.Collections.Generic;

namespace Funkin.NET.Core.Music.Songs
{
    public interface ISection
    {
        bool MustHitSection { get; }

        int TypeOfSection { get; }

        int LengthInSteps { get; }

        List<Note> SectionNotes { get; }

        int? Bpm { get; }

        bool? ChangeBpm { get; }
    }
}