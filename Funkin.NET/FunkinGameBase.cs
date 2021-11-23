using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Funkin.NET.Graphics.Cursor;
using Funkin.NET.Intermediary;
using Funkin.NET.Intermediary.ResourceStores;
using Funkin.NET.Resources;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.IO.Stores;
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

        public override IEnumerable<(ResourceStore<byte[]>, string)> FontStore => new (ResourceStore<byte[]>, string)[]
        {
            (Resources, NET.Resources.Fonts.Torus.TorusBold),
            (Resources, NET.Resources.Fonts.Torus.TorusLight),
            (Resources, NET.Resources.Fonts.Torus.TorusRegular),
            (Resources, NET.Resources.Fonts.Torus.TorusSemiBold)
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

        private Container? OurContent;

        protected override Container<Drawable> Content => OurContent ?? new Container();

        protected virtual BasicCursorContainer? CursorContainer { get; set; }

        [BackgroundDependencyLoader]
        private void Load()
        {
            if (GameDependencies is null)
                throw new NullReferenceException(
                    "Unexpected null reference of GameDependencies. Something is terribly wrong."
                );

            GameDependencies.CacheAs(this);
            GameDependencies.CacheAs(Dependencies);
            GameDependencies.CacheAs(new SparrowAtlasStore(Resources));
            
            Drawable[] mainContent =
            {
                CursorContainer = new BasicCursorContainer
                {
                    RelativeSizeAxes = Axes.Both
                }
            };

            CursorContainer.Child = OurContent = new TooltipContainer(CursorContainer.Cursor)
            {
                RelativeSizeAxes = Axes.Both
            };

            base.Content.Add(CreateScalingContainer().WithChildren(mainContent));
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            GameDependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        protected virtual Container CreateScalingContainer() => new DrawSizePreservingFillContainer();
    }
}