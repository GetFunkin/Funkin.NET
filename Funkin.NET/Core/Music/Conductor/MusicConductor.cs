﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using Funkin.NET.Core.Music.Songs;
using osu.Framework.Logging;

namespace Funkin.NET.Core.Music.Conductor
{
    /// <summary>
    ///     Class responsible for handling song BPM calculations, tracking <see cref="IBpmChange"/><c>s</c>, and some other stuff.
    /// </summary>
    public static class MusicConductor
    {
        public const double DefaultBpm = 100D;
        public const int DefaultSafeFrames = 10;

        public static double Bpm { get; set; } = DefaultBpm;

        public static double CurrentSongPosition
        {
            get => SongPosition + Offset;
            set => SongPosition = value;
        }

        public static double LastSongPosition { get; set; } = 0D;

        public static double Offset { get; set; } = 0D;

        public static int SafeFrames { get; set; } = DefaultSafeFrames;

        public static double SafeZoneOffset => SafeFrames / 60 * 1000; // TODO: hard-coded 60 fps calculation is STUPID

        public static double Crochet => 60 / Bpm * 1000D;

        public static double StepCrochet => Crochet / 4D;

        public static ReadOnlyCollection<IBpmChange> ReadonlyChangeCollection => BpmChangeMap.AsReadOnly();

        private static List<IBpmChange> BpmChangeMap = new();
        private static double SongPosition;

        public static void MapBpmChanges(Song song)
        {
            BpmChangeMap = new List<IBpmChange>();

            double bpm = song.Bpm;
            int totalSteps = 0;
            double totalPos = 0;

            foreach (Section note in song.Sections)
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if ((note.ChangeBpm ?? false) && note.Bpm != bpm)
                {
                    bpm = note.Bpm ?? bpm;
                    BpmChangeMap.Add(new BpmChange(totalSteps, totalPos, bpm));
                }

                int deltaSteps = note.LengthInSteps;
                totalSteps += deltaSteps;
                totalPos += 60D / bpm * 1000D / 4D * deltaSteps;
            }

            Logger.Log($"New BPM map loaded. Change map length: {BpmChangeMap.Count}");
        }
    }
}