namespace Funkin.NET.Content.Conductor
{
    public interface IBpmChange
    {
        int StepTime { get; }

        double SongTime { get; }

        double Bpm { get; }
    }
}