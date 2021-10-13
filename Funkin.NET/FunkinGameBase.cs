using Funkin.NET.Intermediary;
using osu.Framework.Graphics.Containers;

namespace Funkin.NET
{
    public abstract class FunkinGameBase : IntermediaryGame
    {
        public override void OnBackgroundDependencyLoad()
        {
            base.Content.Add(CreateScalingContainer());
        }

        public virtual Container CreateScalingContainer() => new DrawSizePreservingFillContainer();
    }
}