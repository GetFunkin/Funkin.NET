using Funkin.NET.Core.Graphics.UserInterface;
using osu.Framework.Allocation;
using osu.Framework.Graphics;

namespace Funkin.NET.Core.Overlays.KeyBindings
{
    public class ResetButton : FunkinButton
    {
        [BackgroundDependencyLoader]
        private void Load()
        {
            Text = "Reset all binds.";
            RelativeSizeAxes = Axes.X;
            Width = 0.5f;
            Anchor = Anchor.TopCentre;
            Origin = Anchor.TopCentre;
            Margin = new MarginPadding {Top = 15};
            Height = 30;

            Content.CornerRadius = 5;
        }
    }
}