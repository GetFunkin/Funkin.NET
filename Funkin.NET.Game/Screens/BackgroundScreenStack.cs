using System;
using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Screens;

namespace Funkin.NET.Game.Screens
{
    public class BackgroundScreenStack : ScreenStack
    {
        public BackgroundScreenStack()
            : base(false)
        {
            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        /// <summary>
        ///     Attempt to push a new background screen to this stack.
        /// </summary>
        /// <param name="screen">The screen to attempt to push.</param>
        /// <returns>
        ///     Whether the push succeeded. For example, if the existing screen was already of the correct type, this will return <see langword="false"/>.
        /// </returns>
        public virtual bool Push(BackgroundScreen? screen)
        {
            if (screen is null)
                return false;

            if (EqualityComparer<BackgroundScreen>.Default.Equals(CurrentScreen as BackgroundScreen ?? throw new InvalidOperationException(), screen))
                return false;

            base.Push(screen);
            return true;
        }
    }
}
