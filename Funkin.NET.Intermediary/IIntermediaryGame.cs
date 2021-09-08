using osu.Framework.Graphics.Containers;
using osu.Framework.Platform;
using osu.Framework.Screens;

namespace Funkin.NET.Intermediary
{
    public interface IIntermediaryGame
    {
        Storage Storage { get; set; }

        ScreenStack ScreenStack { get; set; }

        void RegisterFonts();

        void InitializeContent();

        void ScreenChanged(IScreen current, IScreen newScreen);

        Container CreateScalingContainer();

        void OnBackgroundDependencyLoad();
    }
}