using osu.Framework.Allocation;

namespace Funkin.NET.Core.BackgroundDependencyLoading
{
    public interface IBackgroundDependencyLoadable
    {
        [BackgroundDependencyLoader]
        void BackgroundDependencyLoad();
    }
}