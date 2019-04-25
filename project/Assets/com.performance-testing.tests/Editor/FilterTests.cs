using System;
using NUnit.Framework;
using PerformanceTesting;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FilterTests
{
    [NUnit.Framework.Test]
    public void Filter_Simple()
    {
        var testTypes = TestSelector.GetTestTypes("One", "Two", "Three");
        List<TestPlan> plans = testTypes.Select(test => new TestPlan(test)).ToList();
        TestRun.Filter(plans, "Void");

        var cases = plans.SelectMany(plan => plan.TestCases).ToArray();

        Assert.AreEqual(cases.Length, 2);
        Assert.IsFalse(Array.Exists(cases, c => c.Name != "Void()" ));
    }

    [NUnit.Framework.Test]
    public void Filter_Multiple()
    {
        var testTypes = TestSelector.GetTestTypes("One", "Two", "Three");
        List<TestPlan> plans = testTypes.Select(test => new TestPlan(test)).ToList();
        TestRun.Filter(plans, "Void", "Enumerator");

        var cases = plans.SelectMany(plan => plan.TestCases).ToArray();

        Assert.AreEqual(cases.Length, 8);
    }

}