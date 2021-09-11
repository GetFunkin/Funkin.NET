using System.Collections.Generic;
using Funkin.NET.Intermediary.Injection;
using Funkin.NET.Intermediary.Utilities;
using osu.Framework.Graphics.Containers;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Framework.Screens;

namespace Funkin.NET.Intermediary
{
    public interface IIntermediaryGame : IServiceHoster, IScheduler
    {
        Storage Storage { get; set; }

        ScreenStack ScreenStack { get; set; }

        IEnumerable<IResourceStore<byte[]>> ResourceStores { get; }

        IEnumerable<(ResourceStore<byte[]>, string)> FontStore { get; }

        void InitializeContent();

        void OnBackgroundDependencyLoad();

        void ScreenChanged(IScreen current, IScreen newScreen);

        Container CreateScalingContainer();
    }
}