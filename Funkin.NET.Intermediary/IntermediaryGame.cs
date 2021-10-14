using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Funkin.NET.Intermediary.Injection.Services;
using Funkin.NET.Intermediary.Utilities;
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
        public Storage? Storage { get; set; }

        public ScreenStack? ScreenStack { get; set; }

        public virtual IEnumerable<IResourceStore<byte[]>> ResourceStores { get; } =
            Array.Empty<ResourceStore<byte[]>>();

        public virtual IEnumerable<(ResourceStore<byte[]>, string)> FontStore { get; } =
            Array.Empty<(ResourceStore<byte[]>, string)>();

        public virtual CastDictionary<ContainerRequest, Container> Containers { get; } = new();

        public abstract Assembly Assembly { get; }

        public IServiceCollection Services { get; } = new ServiceCollection();

        public IServiceProvider ServiceProvider => Services.BuildServiceProvider();

        public virtual void InitializeContent()
        {
        }

        public virtual void ScreenChanged(IScreen current, IScreen newScreen)
        {
        }

        public abstract void InterceptBackgroundDependencyLoad();

        [BackgroundDependencyLoader]
        private void Load()
        {
            foreach (IResourceStore<byte[]> store in ResourceStores)
                Resources.AddStore(store);

            InterceptBackgroundDependencyLoad();

            foreach ((ResourceStore<byte[]> resources, string fontPath) in FontStore)
                AddFont(resources, fontPath);

            foreach (Type type in Assembly.GetTypes())
            {
                if (!typeof(IService).IsAssignableFrom(type) || type.IsAbstract)
                    continue;
                
                MethodInfo[] provider = type.GetMethods().Where(x => x.GetCustomAttribute<ProvidesServiceAttribute>() != null).ToArray();

                IService? service = provider.Length switch
                {
                    > 1 => throw new Exception("Multiple methods marked with ProvidesServiceAttribute."),
                    1 => (IService?) provider[0].Invoke(null, null),
                    _ => (IService?) Activator.CreateInstance(type)
                };

                if (service is null)
                    throw new NullReferenceException($"Failed to create service of type: {type.Name}");

                Services.AddSingleton(type, service);
            }

            InitializeContent();
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            if (ScreenStack is null)
                return;

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
    }
}