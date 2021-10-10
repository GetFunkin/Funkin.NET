using System.Collections.Generic;

namespace Funkin.NET.Core.Music.Songs.Beatmaps
{
    public interface IBeatmap
    {
        BeatmapNotes Notes { get; }

        List<NoteControlPoint> ControlPoints { get; }

        BeatmapInfo Info { get; }
    }
}