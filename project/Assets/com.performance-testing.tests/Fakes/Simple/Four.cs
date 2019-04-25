using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PerformanceTesting;

namespace PerformanceTesting.Tests.Simple
{

    public class Four
    {
        [Test, Case("FirstArg"), Case("SecondArg"), Case("ThirdArg")]
        public void TestCaseMethod(string arg)
        {

        }

        [Test]
        public void SimpleTest()
        {

        }

        [Test]
        public void ValueSource([ValueSource("values")] string value, [ValueSource("values2")] string value2)
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
    }
}
