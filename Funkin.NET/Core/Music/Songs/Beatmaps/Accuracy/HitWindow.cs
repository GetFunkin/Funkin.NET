namespace Funkin.NET.Core.Music.Songs.Beatmaps.Accuracy
{
    public class HitWindow
    {
        public static readonly DifficultyRange[] DefaultRanges =
        {
            new(HitResult.Sick, 64D, 49D, 34D),
            new(HitResult.Good, 97D, 82D, 67D),
            new(HitResult.Bad, 127D, 112D, 97D),
            new(HitResult.Shit, 151D, 136D, 121D),
            new(HitResult.Miss, 188D, 173D, 158D)
        };

        public virtual DifficultyRange[] GetRanges() => DefaultRanges;
    }
}