namespace Funkin.NET.Conductor
{
    /// <summary>
    ///     Base implementation of <see cref="IBpmChange"/>.
    /// </summary>
    public class BpmChange : IBpmChange
    {
        public int StepTime { get; }

        public double SongTime { get; }

        public double Bpm { get; }

        /// <summary>
        /// </summary>
        public BpmChange(int stepTime, double songTime, double bpm)
        {
            StepTime = stepTime;
            SongTime = songTime;
            Bpm = bpm;
        }
    }
}