using System;
using Funkin.NET.Intermediary.Utilities;
using osu.Framework.Screens;

namespace Funkin.NET.Intermediary.Screens.Backgrounds
{
    public interface IBackgroundScreen : IScreen, IScheduler, IEquatable<IBackgroundScreen>, IDisposable
    {
        void ApplyToBackground(Action<IBackgroundScreen> action) => ScheduleTask(() => action.Invoke(this));
    }
}