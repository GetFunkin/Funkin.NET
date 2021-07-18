using System.Collections.Generic;

namespace Funkin.NET.Content.Conductor
{
    public static class MusicConductor
    {
        public const double DefaultBpm = 100D;
        public const int DefaultSafeFrames = 10;

        public static double Bpm = DefaultBpm;
        public static double SongPosition;
        public static double LastSongPosition;
        public static double Offset;
        public static int SafeFrames = DefaultSafeFrames;
        public static double SafeZoneOffset = SafeFrames / 60 * 1000; // TODO: hard-coded 60 fps calculation is STUPID

        public static double Crochet => 60 / Bpm * 1000D;

        public static double StepCrochet => Crochet / 4D;

        public static List<BpmChange> BpmChangeMap = new();

        // TODO: song
        public static void MapBpmChanges()
        {

        }
    }
}