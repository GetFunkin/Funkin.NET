using System.Threading;
using Funkin.NET.Resources;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Threading;
using osu.Framework.Utils;

namespace Funkin.NET.Common.Screens.Background
{
    public class BackgroundScreenDefault : BackgroundScreen
    {
        public osuImpl.Graphics.Backgrounds.Background Background;
        public int CurrentDisplay;
        public const int BackgroundCount = 3;
        public ScheduledDelegate NextTask;
        public CancellationTokenSource CancellationTokenSource;

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

        /// <summary>
        ///     Request loading the next background.
        /// </summary>
        /// <returns>Whether a new background was queued for load. May return false if the current background is still valid.</returns>
        public bool Next()
        {
            osuImpl.Graphics.Backgrounds.Background nextBackground = CreateBackground();

            // in the case that the background hasn't changed, we want to avoid cancelling any tasks that could still be loading.
            if (nextBackground == Background)
                return false;

            CancellationTokenSource?.Cancel();
            CancellationTokenSource = new CancellationTokenSource();

            NextTask?.Cancel();
            NextTask = Scheduler.AddDelayed(() =>
            {
                LoadComponentAsync(nextBackground, DisplayNext, CancellationTokenSource.Token);
            }, 100D);

            return true;
        }

        protected void DisplayNext(osuImpl.Graphics.Backgrounds.Background newBackground)
        {
            Background?.FadeOut(8000D, Easing.InOutSine);
            Background?.Expire();

            AddInternal(Background = newBackground);

            CurrentDisplay++;
        }

        protected osuImpl.Graphics.Backgrounds.Background CreateBackground()
        {
            osuImpl.Graphics.Backgrounds.Background newBackground = new(GetBackgroundTextureName()) {Depth = CurrentDisplay};

            return newBackground;
        }

        protected string GetBackgroundTextureName() =>
            PathHelper.GetTexture($"Backgrounds/menu-background-{CurrentDisplay % BackgroundCount + 1}", includeTextures: false);
    }
}