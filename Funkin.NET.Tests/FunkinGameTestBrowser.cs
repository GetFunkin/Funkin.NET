using System.Reflection;
using Funkin.NET.Intermediary.Screens;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Platform;
using osu.Framework.Testing;

namespace Funkin.NET.Tests
{
    public class FunkinGameTestBrowser : FunkinGameBase
    {
        public override Assembly Assembly => typeof(FunkinGameTestBrowser).Assembly;

        public override void InterceptBackgroundDependencyLoad()
        {
        }

        protected override void LoadComplete()
        {
            // base.LoadComplete();

            LoadComponentAsync(ScreenStack = new DefaultScreenStack(), AddInternal);

            AddRange(new Drawable[]
            {
                new TestBrowser("Funkin.NET.Tests"),
                new CursorContainer()
            });

            // Call after ScreenStack has been set.
            base.LoadComplete();
        }

        public override void SetHost(GameHost host)
        {
            base.SetHost(host);

            host.Window.CursorState |= CursorState.Hidden;
        }
    }
}