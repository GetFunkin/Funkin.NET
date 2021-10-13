using osu.Framework.Testing;

namespace Funkin.NET.Tests.Visual
{
    [ExcludeFromDynamicCompile]
    public abstract class FunkinTestScene : TestScene
    {
        protected override ITestSceneTestRunner CreateRunner() => new FunkinTestSceneTestRunner();
    }
}