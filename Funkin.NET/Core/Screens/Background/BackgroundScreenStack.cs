using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osuTK;

// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.Core.Screens.Background
{
    /// <summary>
    ///     See: osu!'s BackgroundScreenStack.
    /// </summary>
    public class BackgroundScreenStack : ScreenStack
    {
        public BackgroundScreenStack() : base(false)
        {
            Scale = new Vector2(1.06f);
            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        public void Push(BackgroundScreen screen)
        {
            if (screen == null)
                return;

            if (EqualityComparer<BackgroundScreen>.Default.Equals((BackgroundScreen) CurrentScreen, screen))
                return;

            base.Push(screen);
        }
    }
}