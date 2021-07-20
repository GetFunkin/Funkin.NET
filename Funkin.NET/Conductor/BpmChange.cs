namespace Funkin.NET.Conductor
{
    public class BpmChange : IBpmChange
    {
        public int StepTime { get; }

        public double SongTime { get; }

        public double Bpm { get; }

        public BpmChange(int stepTime, double songTime, double bpm)
        {
            StepTime = stepTime;
            SongTime = songTime;
            Bpm = bpm;
        }
    }
}