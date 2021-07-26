using System.Threading;
using Funkin.NET.Resources;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Threading;
using osu.Framework.Utils;

namespace Funkin.NET.Screens.Background
{
    /// <summary>
    ///     See: osu!'s BackgroundScreenDefault.
    /// </summary>
    public class BackgroundScreenDefault : BackgroundScreen
    {
        private Graphics.Backgrounds.Background _background;
        private int _currentDisplay;
        private const int BackgroundCount = 3;
        private ScheduledDelegate _nextTask;
        private CancellationTokenSource _cancellationTokenSource;

        public BackgroundScreenDefault(bool animateOnEnter = true)
            : base(animateOnEnter)
        {
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            _currentDisplay = RNG.Next(0, BackgroundCount);

            Next();
        }

        /// <summary>
        ///     Request loading the next background.
        /// </summary>
        /// <returns>Whether a new background was queued for load. May return false if the current background is still valid.</returns>
        public bool Next()
        {
            Graphics.Backgrounds.Background nextBackground = CreateBackground();

            // in the case that the background hasn't changed, we want to avoid cancelling any tasks that could still be loading.
            if (nextBackground == _background)
                return false;

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            _nextTask?.Cancel();
            _nextTask = Scheduler.AddDelayed(
                () => { LoadComponentAsync(nextBackground, DisplayNext, _cancellationTokenSource.Token); }, 100D);

            return true;
        }

        private void DisplayNext(Graphics.Backgrounds.Background newBackground)
        {
            _background?.FadeOut(8000D, Easing.InOutSine);
            _background?.Expire();

            AddInternal(_background = newBackground);

            _currentDisplay++;
        }

        private Graphics.Backgrounds.Background CreateBackground()
        {
            Graphics.Backgrounds.Background newBackground = new(GetBackgroundTextureName()) {Depth = _currentDisplay};

            return newBackground;
        }

        private string GetBackgroundTextureName() =>
            PathHelper.GetTexture($"Backgrounds/menu-background-{_currentDisplay % BackgroundCount + 1}");
    }
}