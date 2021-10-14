using System.Reflection;
using Funkin.NET.Intermediary.Screens;

namespace Funkin.NET
{
    public class FunkinGame : FunkinGameBase
    {
        public override Assembly Assembly => typeof(FunkinGame).Assembly;

        protected override void LoadComplete()
        {
            LoadComponentAsync(ScreenStack = new DefaultScreenStack(), AddInternal);

            base.LoadComplete();
        }
    }
}