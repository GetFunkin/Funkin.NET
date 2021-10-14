using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Funkin.NET.Intermediary;
using Funkin.NET.Resources;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.IO.Stores;
using osu.Framework.Screens;
using AssemblyAttribute = Funkin.NET.Properties.AssemblyAttributes.AssemblyConfigurationAttribute;

namespace Funkin.NET
{
    public abstract class FunkinGameBase : IntermediaryGame
    {
        public const string ClientName = "funkin.net";

        public virtual Version AssemblyVersion => Assembly.GetName().Version ?? new Version();

        public DependencyContainer? GameDependencies { get; protected set; }

        public override IEnumerable<IResourceStore<byte[]>> ResourceStores => new[]
        {
            new DllResourceStore(PathHelper.Assembly)
        };

        public virtual string Version
        {
            get
            {
                AssemblyAttribute? attrib = Assembly.GetCustomAttribute<AssemblyAttribute>();

                if (!Debugger.IsAttached)
                    return $"{ClientName} Release {AssemblyVersion}";

                if (attrib is null)
                    return AssemblyVersion + " -debug";

                return $"{ClientName}-{AssemblyVersion} {attrib.Configuration} {attrib.Platform}";
            }
        }

        public override void InterceptBackgroundDependencyLoad()
        {
            base.Content.Add(CreateScalingContainer());
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            if (GameDependencies is null)
                throw new NullReferenceException(
                    "Unexpected null reference of GameDependencies. Something is terribly wrong."
                );

            GameDependencies.CacheAs(this);
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            GameDependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        public virtual Container CreateScalingContainer() => new DrawSizePreservingFillContainer();
    }
}