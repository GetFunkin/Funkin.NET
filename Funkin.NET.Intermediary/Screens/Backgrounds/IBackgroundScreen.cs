using System;
using Funkin.NET.Intermediary.Utilities;
using osu.Framework.Screens;

namespace Funkin.NET.Intermediary.Screens.Backgrounds
{
    public interface IBackgroundScreen : IScreen, IEquatable<IBackgroundScreen>, IDisposable
    {
        void ApplyToBackground(Action<IBackgroundScreen> action);
    }
}