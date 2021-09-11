using System;

namespace Funkin.NET.Intermediary.Utilities
{
    /// <summary>
    ///     Schedules <see cref="Action"/>s.
    /// </summary>
    public interface IScheduler
    {
        /// <summary>
        ///     <see cref="Action"/> scheduler. Mainly used for exposing pre-existing schedulers.
        /// </summary>
        void ScheduleTask(Action action);
    }
}