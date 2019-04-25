using System.Collections;
using UnityEngine;
using PerformanceTesting;

namespace PerformanceTesting.Tests.Simple
{
    public class DebugSetup : IPrebuildSetup
    {
        public void Setup()
        {
        }
    }

    public class Three
    {
        [PrebuildSetup]
        public void PrebuildSetup()
        {
        }

        [PrebuildSetup]
        public IEnumerator PrebuildSetup_IEnumerator()
        {
            yield return null;
        }

        public IEnumerator WaitForFrame()
        {
            yield return null;
        }

        [OneTimeSetup]
        public void OneTimeSetup()
        {
        }

        [OneTimeSetup]
        public IEnumerator OneTimeSetup_Enumerator()
        {
            yield return null;
        }

        [OneTimeTeardown]
        public void OneTimeTeardown()
        {
        }

        [OneTimeTeardown]
        public IEnumerator OneTimeTeardown_Enumerator()
        {
            yield return null;
        }

        [Setup]
        public void Setup()
        {
        }

        [Setup]
        public IEnumerator SetupEnumerator()
        {
            yield return null;
        }

        [Teardown]
        public void Teardown()
        {
        }

        [Teardown]
        public IEnumerator TeardownEnumerator()
        {
            yield return null;
        }

        [PrebuildSetup("DebugSetup")]
        [Test]
        public void Debug_Log()
        {
        }

        [Test]
        public void Debug_Log_UnityTest()
        {
        }

        [Test, Case("TestCase1"), Case("TestCase2")]
        public void Debug_Log(string message)
        {
        }


        public static string[] values()
        {
            return new[] { "VALUE1", "VALUE2" };
        }

        public static string[] values2()
        {
            return new[] { "VALUE1", "VALUE2", "VALUE3" };
        }

        [Test]
        public void Debug_Log_FromSourceValue([ValueSource("values")] string value, [ValueSource("values2")] string value2)
        {
        }

        [Test, Case("TestCase1", "TestCase2")]
        public void Debug_Log(string message, string message2)
        {
        }

        [Test]
        public IEnumerator IEnumerator_Log()
        {
            yield return null;
        }

        [Test, Case("IEnumerator.TestCase1"), Case("IEnumerator.TestCase2")]
        public IEnumerator IEnumerator_Log(string name)
        {
            yield return null;
        }

        [Test, Case("IEnumerator.TestCase2_1", "IEnumerator.TestCase2_2")]
        public IEnumerator IEnumerator_Log(string name, string name2)
        {
            yield return null;
        }
    }
}
