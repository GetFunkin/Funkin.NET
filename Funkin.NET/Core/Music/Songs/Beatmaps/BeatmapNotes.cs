using System.Collections.Generic;

namespace Funkin.NET.Core.Music.Songs.Beatmaps
{
    public class BeatmapNotes
    {
        public List<BeatmapObject> NoteObjects { get; }

        public BeatmapNotes(List<BeatmapObject> noteObjects)
        {
            NoteObjects = noteObjects;
        }
    }
}