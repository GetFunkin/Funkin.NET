using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Platform;
using osu.Framework.Testing;

namespace Funkin.Game.Tests
{
    public class FunkinTestBrowser : FunkinGameBase
    {
        protected override void LoadComplete()
        {
            base.LoadComplete();

            AddRange(new Drawable[]
            {
                new TestBrowser("Funkin"),
                new CursorContainer()
            });
        }

        public override void SetHost(GameHost host)
        {
            base.SetHost(host);
            host.Window.CursorState |= CursorState.Hidden;
        }
    }
}
