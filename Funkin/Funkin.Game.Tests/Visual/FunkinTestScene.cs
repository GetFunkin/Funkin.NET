using osu.Framework.Testing;

namespace Funkin.Game.Tests.Visual
{
    public class FunkinTestScene : TestScene
    {
        protected override ITestSceneTestRunner CreateRunner() => new FunkinTestSceneTestRunner();

        private class FunkinTestSceneTestRunner : FunkinGameBase, ITestSceneTestRunner
        {
            private TestSceneTestRunner.TestRunner runner;

            protected override void LoadAsyncComplete()
            {
                base.LoadAsyncComplete();
                Add(runner = new TestSceneTestRunner.TestRunner());
            }

            public void RunTestBlocking(TestScene test) => runner.RunTestBlocking(test);
        }
    }
}
