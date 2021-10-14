﻿using System.Reflection;
using osu.Framework.Testing;

namespace Funkin.NET.Tests.Visual
{
    public class FunkinTestSceneTestRunner : FunkinGameBase, ITestSceneTestRunner
    {
        public override Assembly Assembly => typeof(FunkinGameBase).Assembly;

        private TestSceneTestRunner.TestRunner Runner;

        public override void InterceptBackgroundDependencyLoad()
        {
            base.InterceptBackgroundDependencyLoad();

            Add(Runner = new TestSceneTestRunner.TestRunner());
        }

        public void RunTestBlocking(TestScene test) => Runner.RunTestBlocking(test);
    }
}