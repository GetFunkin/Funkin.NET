using Funkin.NET.Core.Music.Songs.Beatmaps;

namespace Funkin.NET.Core.Music.Songs.Legacy.Beatmaps
{
    public class BpmChangeEvent : NoteControlPoint
    {
        public double NewBpm { get; set; }


        public override void OnHit(IBeatmap beatmap)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (beatmap.Info.Bpm != NewBpm)
                beatmap.Info.Bpm = NewBpm;
        }
    }
}