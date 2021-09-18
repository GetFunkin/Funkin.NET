using System.Collections.Generic;

namespace Funkin.NET.Core.Music.Songs
{
    public interface ISong
    {
        string Player1 { get; }

        string Player2 { get; }

        double Speed { get; }

        bool NeedsVoices { get; }

        List<object> SectionLengths { get; }

        string SongName { get; }

        List<ISection> Sections { get; }

        double Bpm { get; }

        int NumSections { get; }
    }
}