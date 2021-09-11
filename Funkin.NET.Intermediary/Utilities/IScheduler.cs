using System;

namespace Funkin.NET.Intermediary.Utilities
{
    public interface IScheduler
    {
        void ScheduleTask(Action action);
    }
}