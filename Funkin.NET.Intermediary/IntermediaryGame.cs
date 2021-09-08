using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;

namespace Funkin.NET.Intermediary
{
    public abstract class IntermediaryGame : Game, IIntermediaryGame
    {
        public Storage Storage { get; set; }

        public ScreenStack ScreenStack { get; set; }

        public abstract void RegisterFonts();

        public virtual void InitializeContent()
        {
        }

        public virtual void ScreenChanged(IScreen current, IScreen newScreen)
        {
        }

        public abstract Container CreateScalingContainer();

        public abstract void OnBackgroundDependencyLoad();

        [BackgroundDependencyLoader]
        private void Load()
        {
            OnBackgroundDependencyLoad();

            RegisterFonts();
            InitializeContent();
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            ScreenStack.ScreenPushed += ScreenPushed;
            ScreenStack.ScreenExited += ScreenExited;
        }

        private void ScreenPushed(IScreen lastScreen, IScreen newScreen)
        {
            ScreenChanged(lastScreen, newScreen);
            Logger.Log($"Screen ({lastScreen}) pushed to → {newScreen}");
        }

        private void ScreenExited(IScreen lastScreen, IScreen newScreen)
        {
            ScreenChanged(lastScreen, newScreen);
            Logger.Log($"Screen ({lastScreen}) exited to → {newScreen}");

            // todo: set to FunnyTextScreen exit edition
            if (newScreen == null)
                Exit();
        }
    }
}