using System.Collections.Generic;
using System.Collections.ObjectModel;
using Funkin.NET.Core.Music.Songs;
using Funkin.NET.Core.Music.Songs.Legacy;
using osu.Framework.Logging;

namespace Funkin.NET.Core.Music.Conductor
{
    /// <summary>
    ///     Class responsible for handling song BPM calculations, tracking <see cref="IBpmChange"/><c>s</c>, and some other stuff.
    /// </summary>
    public class MusicConductor
    {
        public const int DefaultSafeFrames = 10;

        protected double RealBpm;

        public virtual double Bpm
        {
            get => RealBpm;

            set => RealBpm = value;
        }

        public virtual double CurrentSongPosition
        {
            get => SongPosition + Offset;
            set => SongPosition = value;
        }

        public virtual double LastSongPosition { get; set; } = 0D;

        public virtual double Offset { get; set; } = 0D;

        public virtual int SafeFrames { get; set; } = DefaultSafeFrames;

        public virtual double SafeZoneOffset => SafeFrames / 60 * 1000; // TODO: hard-coded 60 fps calculation is STUPID

        public virtual double Crochet => 60 / Bpm * 1000D;

        public virtual double StepCrochet => Crochet / 4D;

        public virtual ReadOnlyCollection<IBpmChange> ReadonlyChangeCollection => BpmChangeMap.AsReadOnly();

        protected List<IBpmChange> BpmChangeMap = new();
        protected double SongPosition;

        public MusicConductor(double bpm)
        {
            RealBpm = bpm;
        }

        public virtual void MapBpmChanges(ISong legacySong)
        {
            BpmChangeMap = new List<IBpmChange>();

            double bpm = legacySong.Bpm;
            int totalSteps = 0;
            double totalPos = 0;

            foreach (ISection noteSection in legacySong.Sections)
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if ((noteSection.ChangeBpm ?? false) && noteSection.Bpm != bpm)
                {
                    bpm = noteSection.Bpm ?? bpm;
                    BpmChangeMap.Add(new BpmChange(totalSteps, totalPos, bpm));
                }

                int deltaSteps = noteSection.LengthInSteps;
                totalSteps += deltaSteps;
                totalPos += 60D / bpm * 1000D / 4D * deltaSteps;
            }

            Logger.Log($"New BPM map loaded. Change map length: {BpmChangeMap.Count}");
        }
    }
}