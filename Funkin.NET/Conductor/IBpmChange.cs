namespace Funkin.NET.Conductor
{
    public interface IBpmChange
    {
        int StepTime { get; }

        double SongTime { get; }

        double Bpm { get; }
    }
}