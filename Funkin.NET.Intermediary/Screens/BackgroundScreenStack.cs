using Funkin.NET.Intermediary.Screens.Backgrounds;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osuTK;
// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.Intermediary.Screens
{
    public class BackgroundScreenStack : ScreenStack
    {
        public BackgroundScreenStack() : base(false)
        {
            Scale = new Vector2(1.06f);
            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        public virtual void Push(IBackgroundScreen screen)
        {
            if (screen is null)
                return;

            base.Push(screen);
        }
    }
}