namespace Funkin.NET.Core.Conductor
{
    /// <summary>
    ///     Basic BPM change interface.
    /// </summary>
    public interface IBpmChange
    {
        /// <summary>
        ///     The current step. By default, the current step is <see cref="StepTime"/> plus the floored crochet.
        /// </summary>
        int StepTime { get; }

        /// <summary>
        ///     The time at which this BPM change is located in the song.
        /// </summary>
        double SongTime { get; }

        /// <summary>
        ///     The current BPM.
        /// </summary>
        double Bpm { get; }
    }
}