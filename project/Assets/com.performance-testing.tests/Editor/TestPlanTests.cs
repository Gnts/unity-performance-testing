using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PerformanceTesting;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestPlanTests
{
    [NUnit.Framework.Test]
    public void Populate_One()
    {
        var plan = new TestPlan("One");

        Assert.AreEqual(plan.TestCases.Count, 2);
        Assert.IsTrue(plan.TestCases.Any(t => t.Name == "Void()"));
        Assert.IsTrue(plan.TestCases.Any(t => t.Name == "Enumerator()"));

        Assert.AreEqual(plan.Setups.Length, 2, "Setups were not populated.");
        Assert.AreEqual(plan.Setups[0].Name, "Setup_Void");
        Assert.AreEqual(plan.Setups[1].Name, "Setup_Enumerator");

        Assert.AreEqual(plan.Teardowns.Length, 0);
        Assert.AreEqual(plan.OneTimeSetups.Length, 0);
        Assert.AreEqual(plan.OneTimeTearDowns.Length, 0);
    }

    [NUnit.Framework.Test]
    public void Populate_Two()
    {
        var plan = new TestPlan("Two");

        Assert.AreEqual(plan.TestCases.Count, 2);
        Assert.IsTrue(plan.TestCases.Any(t => t.Name == "Void()"));
        Assert.IsTrue(plan.TestCases.Any(t => t.Name == "Enumerator()"));

        Assert.AreEqual(plan.Setups.Length, 2);
        Assert.AreEqual(plan.Setups[0].Name, "Setup_Void");
        Assert.AreEqual(plan.Setups[1].Name, "Setup_Enumerator");

        Assert.AreEqual(plan.Teardowns.Length, 0);
        Assert.AreEqual(plan.OneTimeSetups.Length, 0);
        Assert.AreEqual(plan.OneTimeTearDowns.Length, 0);
    }

    [NUnit.Framework.Test]
    public void Populate_Three()
    {
        var plan = new TestPlan("Three");

        Assert.AreEqual(plan.TestCases.Count, 15);
        Assert.IsTrue(plan.TestCases.Any(t => t.Name == "Debug_Log()"));
        Assert.IsTrue(plan.TestCases.Any(t => t.Name == "Debug_Log_UnityTest()"));
        Assert.IsTrue(plan.TestCases.Any(t => t.Name == "Debug_Log(TestCase1)"));
        Assert.IsTrue(plan.TestCases.Any(t => t.Name == "Debug_Log(TestCase2)"));
        Assert.IsTrue(plan.TestCases.Any(t => t.Name == "Debug_Log_FromSourceValue(VALUE1, VALUE1)"));
        Assert.IsTrue(plan.TestCases.Any(t => t.Name == "Debug_Log_FromSourceValue(VALUE1, VALUE2)"));
        Assert.IsTrue(plan.TestCases.Any(t => t.Name == "Debug_Log_FromSourceValue(VALUE1, VALUE3)"));
        Assert.IsTrue(plan.TestCases.Any(t => t.Name == "Debug_Log_FromSourceValue(VALUE2, VALUE1)"));
        Assert.IsTrue(plan.TestCases.Any(t => t.Name == "Debug_Log_FromSourceValue(VALUE2, VALUE2)"));
        Assert.IsTrue(plan.TestCases.Any(t => t.Name == "Debug_Log_FromSourceValue(VALUE2, VALUE3)"));
        Assert.IsTrue(plan.TestCases.Any(t => t.Name == "Debug_Log(TestCase1, TestCase2)"));
        Assert.IsTrue(plan.TestCases.Any(t => t.Name == "IEnumerator_Log()"));
        Assert.IsTrue(plan.TestCases.Any(t => t.Name == "IEnumerator_Log(IEnumerator.TestCase1)"));
        Assert.IsTrue(plan.TestCases.Any(t => t.Name == "IEnumerator_Log(IEnumerator.TestCase2)"));
        Assert.IsTrue(plan.TestCases.Any(t => t.Name == "IEnumerator_Log(IEnumerator.TestCase2_1, IEnumerator.TestCase2_2)"));


        Assert.AreEqual(plan.Setups.Length, 2);
        Assert.AreEqual(plan.Setups[0].Name, "Setup");
        Assert.AreEqual(plan.Setups[1].Name, "SetupEnumerator");

        Assert.AreEqual(plan.Teardowns.Length, 2);
        Assert.AreEqual(plan.Teardowns[0].Name, "Teardown");
        Assert.AreEqual(plan.Teardowns[1].Name, "TeardownEnumerator");

        Assert.AreEqual(plan.OneTimeSetups.Length, 2);
        Assert.AreEqual(plan.OneTimeSetups[0].Name, "OneTimeSetup");
        Assert.AreEqual(plan.OneTimeSetups[1].Name, "OneTimeSetup_Enumerator");

        Assert.AreEqual(plan.OneTimeTearDowns.Length, 2);
        Assert.AreEqual(plan.OneTimeTearDowns[0].Name, "OneTimeTeardown");
        Assert.AreEqual(plan.OneTimeTearDowns[1].Name, "OneTimeTeardown_Enumerator");
    }
}