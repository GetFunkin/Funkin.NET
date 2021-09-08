namespace Funkin.NET.Core.Music.Conductor
{
    /// <summary>
    ///     Base implementation of <see cref="IBpmChange"/>.
    /// </summary>
    public class BpmChange : IBpmChange
    {
        public virtual int StepTime { get; }

        public virtual double SongTime { get; }

        public virtual double Bpm { get; }

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