using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PerformanceTesting;

namespace PerformanceTesting
{
    public class TestPlan
    {
        public TestPlan(Type type)
        {
            TestType = type;
            Populate();
        }

        public TestPlan(string type)
        {
            TestType = TestSelector.GetTestType(type);
            Populate();
        }

        public Type TestType { get; private set; }
        public object Instance { get; private set; }
        public MethodInfo[] OneTimeSetups { get; private set; }
        public MethodInfo[] OneTimeTearDowns { get; private set; }
        public MethodInfo[] Setups { get; private set; }
        public MethodInfo[] Teardowns { get; private set; }
        public MethodInfo[] TestMethods { get; private set; }
        public List<TestCase> TestCases { get; internal set; }

        private void Populate()
        {
            TestCases = new List<TestCase>();

            OneTimeSetups = TestType.GetMethodsWithAttribute<OneTimeSetup>();
            OneTimeTearDowns = TestType.GetMethodsWithAttribute<OneTimeTeardown>();

            Setups = TestType.GetMethodsWithAttribute<Setup>();
            Teardowns = TestType.GetMethodsWithAttribute<Teardown>();

            TestMethods = TestType.GetMethodsWithAttribute<Test>();

            if (!TestMethods.Any()) return;
            Instance = Activator.CreateInstance(TestType);

            foreach (var method in TestMethods)
                TestCases.AddRange(TestSelector.GetTestCasesForMethod(Instance, TestType, method));
        }

        public IEnumerator Execute()
        {
            if (!TestMethods.Any()) yield break;

            yield return Instance.Invoke(OneTimeSetups);

            foreach (var testCase in TestCases)
            {
                UnityEngine.Debug.LogFormat("Starting test {0}", testCase.Name);
                yield return Instance.Invoke(Setups);
                TestRun.TestStarted(testCase);
                yield return testCase.Execute(Instance);
                TestRun.TestFinished(testCase);
                yield return Instance.Invoke(Teardowns);
            }
        }
    }
}