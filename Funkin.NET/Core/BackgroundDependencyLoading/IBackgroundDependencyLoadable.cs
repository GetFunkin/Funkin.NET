using Funkin.NET.Content.Configuration;

namespace Funkin.NET.Core.BackgroundDependencyLoading
{
    public interface IBackgroundDependencyLoadable
    {
        void BackgroundDependencyLoad()
        {
        }

        void BackgroundDependencyLoad(FunkinConfigManager config)
        {
        }
    }
}