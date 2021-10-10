using System.Collections.Generic;
using System.Linq;
using Funkin.NET.Core.Music.Songs.Beatmaps;

namespace Funkin.NET.Core.Music.Songs.Legacy.Beatmaps
{
    public class LegacyBeatmap : IBeatmap
    {
        public BeatmapNotes Notes { get; }

        public List<NoteControlPoint> ControlPoints { get; }

        public BeatmapInfo Info { get; }

        public LegacyBeatmap(LegacyTrack song)
        {
            List<NoteControlPoint> legacyControlPoints = (
                from section
                    in song.Song.Sections
                where section.ChangeBpm.HasValue && section.ChangeBpm.Value
                select new BpmChangeEvent
                {
                    NewBpm = section.Bpm ?? 0
                }
            ).Cast<NoteControlPoint>().ToList();

            ControlPoints = legacyControlPoints;

            Info = new BeatmapInfo
            {
                Author = "Unknown",
                Title = song.Song.SongName,
                Bpm = song.Bpm,
                Length = -1D,
                StartOffset = 0D
            };

            List<BeatmapObject> notes = (
                from section
                    in song.Song.Sections
                from note
                    in section.SectionNotes
                select new BeatmapObject
                {
                    StartTime = note.Item1,
                    HoldTime = note.Item3
                }
            ).ToList();

            Notes = new BeatmapNotes(notes);
        }
    }
}