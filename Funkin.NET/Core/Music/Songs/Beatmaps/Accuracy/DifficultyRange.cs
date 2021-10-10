namespace Funkin.NET.Core.Music.Songs.Beatmaps.Accuracy
{
    public struct DifficultyRange
    {
        public HitResult Result;

        public double Min;
        public double Average;
        public double Max;

        public DifficultyRange(HitResult result, double min, double average, double max)
        {
            Result = result;
            Min = min;
            Average = average;
            Max = max;
        }
    }
}