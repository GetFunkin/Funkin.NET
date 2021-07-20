namespace Funkin.NET.Songs
{
    public interface ISong<out TNote> where TNote : INote
    {
        string SongName { get; }

        TNote[] Notes { get; }

        double Bpm { get; }

        bool NeedsVoices { get; }

        float Speed { get; }

        // TODO: move away from string-based characters
        // TODO: in favor of an OOP-ish character setup
        string PlayerCharacter { get; }

        string OpponentCharacter { get; }

        bool ValidScore { get; }
    }

    public interface ISong : ISong<INote>
    {
    }
}