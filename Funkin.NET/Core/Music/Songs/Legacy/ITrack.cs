using System.Collections.Generic;

namespace Funkin.NET.Core.Music.Songs.Legacy
{
    public interface ITrack
    {
        ISong Song { get; }

        double Bpm { get; }

        int Sections { get; }

        List<ISection> Notes { get; }
    }
}