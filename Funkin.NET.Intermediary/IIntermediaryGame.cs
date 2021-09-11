using System.Collections.Generic;
using Funkin.NET.Intermediary.Injection;
using osu.Framework.Graphics.Containers;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Framework.Screens;

namespace Funkin.NET.Intermediary
{
    public interface IIntermediaryGame : IServiceHoster
    {
        Storage Storage { get; set; }

        ScreenStack ScreenStack { get; set; }

        IEnumerable<(ResourceStore<byte[]>, string)> Fonts { get; }

        void InitializeContent();

        void OnBackgroundDependencyLoad();

        void ScreenChanged(IScreen current, IScreen newScreen);

        Container CreateScalingContainer();
    }
}