using System.Threading;
using Funkin.NET.Content.osu.Graphics.Backgrounds;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Threading;
using osu.Framework.Utils;

namespace Funkin.NET.Content.osu.Screens.Backgrounds
{
    /// <summary>
    ///     Heavily-modified version of osu!'s BackgroundScreenDefault.
    /// </summary>
    public class BackgroundScreenDefault : BackgroundScreen
    {
        public Background Background;

        public int CurrentDisplay;
        public const int BackgroundCount = 1;

        protected virtual bool AllowStoryboardBackground => true;

        public BackgroundScreenDefault(bool animateOnEnter = true)
            : base(animateOnEnter)
        {
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            CurrentDisplay = RNG.Next(0, BackgroundCount);

            Next();
        }

        public ScheduledDelegate NextTask;
        public CancellationTokenSource CancellationTokenSource;

        /// <summary>
        /// Request loading the next background.
        /// </summary>
        /// <returns>Whether a new background was queued for load. May return false if the current background is still valid.</returns>
        public bool Next()
        {
            Background nextBackground = CreateBackground();

            // in the case that the background hasn't changed, we want to avoid cancelling any tasks that could still be loading.
            if (nextBackground == Background)
                return false;

            CancellationTokenSource?.Cancel();
            CancellationTokenSource = new CancellationTokenSource();

            NextTask?.Cancel();
            NextTask = Scheduler.AddDelayed(
                () => { LoadComponentAsync(nextBackground, DisplayNext, CancellationTokenSource.Token); }, 100);

            return true;
        }

        private void DisplayNext(Background newBackground)
        {
            Background?.FadeOut(800, Easing.InOutSine);
            Background?.Expire();

            AddInternal(Background = newBackground);
            CurrentDisplay++;
        }

        private Background CreateBackground()
        {
            Background newBackground = new(GetBackgroundTextureName());

            if (newBackground.Equals(Background))
                return Background;

            newBackground.Depth = CurrentDisplay;

            return newBackground;
        }

        private static string GetBackgroundTextureName()
        {
            return "Menu/menu-background-1";
        }
    }
}