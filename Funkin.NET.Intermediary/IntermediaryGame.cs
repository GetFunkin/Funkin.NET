using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;

namespace Funkin.NET.Intermediary
{
    public abstract class IntermediaryGame : Game, IIntermediaryGame
    {
        public Storage Storage { get; set; }

        public ScreenStack ScreenStack { get; set; }

        public virtual IEnumerable<IResourceStore<byte[]>> ResourceStores { get; } =
            Array.Empty<ResourceStore<byte[]>>();

        public virtual IEnumerable<(ResourceStore<byte[]>, string)> FontStore { get; } =
            Array.Empty<(ResourceStore<byte[]>, string)>();

        public IServiceCollection Services { get; } = new ServiceCollection();

        public IServiceProvider ServiceProvider => Services.BuildServiceProvider();

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
            foreach (IResourceStore<byte[]> store in ResourceStores)
                Resources.AddStore(store);

            OnBackgroundDependencyLoad();

            foreach ((ResourceStore<byte[]> resources, string fontPath) in FontStore)
                AddFont(resources, fontPath);

            InitializeContent();
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            ScreenStack.ScreenPushed += ScreenPushed;
            ScreenStack.ScreenExited += ScreenExited;
        }

        protected virtual void ScreenPushed(IScreen lastScreen, IScreen newScreen)
        {
            ScreenChanged(lastScreen, newScreen);
            Logger.Log($"Screen ({lastScreen}) pushed to → {newScreen}");
        }

        protected virtual void ScreenExited(IScreen lastScreen, IScreen newScreen)
        {
            ScreenChanged(lastScreen, newScreen);
            Logger.Log($"Screen ({lastScreen}) exited to → {newScreen}");

            if (newScreen == null)
                Exit();
        }

        // serves the purpose of exposing scheduling to outside members
        // why is this normally protected, peppy? please...
        public virtual void ScheduleTask(Action action) => Schedule(action);
    }
}