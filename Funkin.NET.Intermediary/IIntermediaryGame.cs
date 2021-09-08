using Funkin.NET.Intermediary.Injection;
using osu.Framework.Graphics.Containers;
using osu.Framework.Platform;
using osu.Framework.Screens;

namespace Funkin.NET.Intermediary
{
    public interface IIntermediaryGame : IServiceHoster
    {
        Storage Storage { get; set; }

        ScreenStack ScreenStack { get; set; }

        void InitializeContent();

        void OnBackgroundDependencyLoad();

        void RegisterFonts();

        void ScreenChanged(IScreen current, IScreen newScreen);

        Container CreateScalingContainer();
    }
}