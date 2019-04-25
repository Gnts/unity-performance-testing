using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PerformanceTesting;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestSelectorTests
{
    [NUnit.Framework.Test]
    public void GetTestTypesFromASsembly()
    {
        var types = TestSelector.GetTestTypesFromAssembly("PerformanceTesting.Tests.Simple");

        Assert.AreEqual(types.Count, 4);
        Assert.IsTrue(types.Any(t => t.Name == "One"));
        Assert.IsTrue(types.Any(t => t.Name == "Two"));
        Assert.IsTrue(types.Any(t => t.Name == "Three"));
        Assert.IsTrue(types.Any(t => t.Name == "Four"));
    }

    [NUnit.Framework.Test]
    public void GetTestTypes_Null_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() => TestSelector.GetTestTypes());
    }

    [NUnit.Framework.Test]
    public void GetTestTypes()
    {
        var types = TestSelector.GetTestTypes("One", "Two", "Three", "Four");

        Assert.AreEqual(types.Count, 4);
        Assert.IsTrue(types.Any(t => t.Name == "One"));
        Assert.IsTrue(types.Any(t => t.Name == "Two"));
        Assert.IsTrue(types.Any(t => t.Name == "Three"));
        Assert.IsTrue(types.Any(t => t.Name == "Four"));
    }

    [NUnit.Framework.Test]
    public void GetTestAssemblies_Null_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() => TestSelector.GetTestAssemblies());
    }

    [NUnit.Framework.Test]
    public void GetTestAssemblies()
    {
        var types = TestSelector.GetTestAssemblies("PerformanceTesting.Tests.Simple");

        Assert.AreEqual(types.Count, 1);
        Assert.IsTrue(types.Any(t => t.FullName.Contains("PerformanceTesting.Tests.Simple")));
    }


    [NUnit.Framework.Test]
    public void GetTestTypesFromAssemblies_Null_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() => TestSelector.GetTestTypesFromAssemblies());
    }


    [NUnit.Framework.Test]
    public void GetTestTypesFromAssemblies()
    {
        var types = TestSelector.GetTestTypesFromAssemblies("PerformanceTesting.Tests.Simple");

        Assert.AreEqual(types.Count, 4);
        Assert.IsTrue(types.Any(t => t.Name == "One"));
        Assert.IsTrue(types.Any(t => t.Name == "Two"));
        Assert.IsTrue(types.Any(t => t.Name == "Three"));
        Assert.IsTrue(types.Any(t => t.Name == "Four"));
    }

    [NUnit.Framework.Test]
    public void GetTestAssembly()
    {
        var type = TestSelector.GetTestAssembly("PerformanceTesting.Tests.Simple");

        Assert.NotNull(type);
        Assert.IsTrue(type.FullName.Contains("PerformanceTesting.Tests.Simple"));
    }

    [NUnit.Framework.Test]
    public void GetMethodsWithAttribute()
    {

        var testType = TestSelector.GetTestType("One");
        var testMethods = testType.GetMethodsWithAttribute<Test>();

        Assert.AreEqual(testMethods.Length, 2);
        Assert.IsTrue(testMethods.Any(t => t.Name == "Void"));
        Assert.IsTrue(testMethods.Any(t => t.Name == "Enumerator"));
    }

    [NUnit.Framework.Test]
    public void GetMethodsWithAttributeTest()
    {
        var testType = TestSelector.GetTestType("Three");
        var testMethods = testType.GetMethodsWithAttribute<Test>();

        Assert.AreEqual(testMethods.Length, 8);
        Assert.IsTrue(testMethods.Any(t => t.Name == "Debug_Log"));
        Assert.IsTrue(testMethods.Any(t => t.Name == "Debug_Log_UnityTest"));
        Assert.IsTrue(testMethods.Any(t => t.Name == "Debug_Log_FromSourceValue"));
        Assert.IsTrue(testMethods.Any(t => t.Name == "IEnumerator_Log"));
    }

    [NUnit.Framework.Test]
    public void GetMethodsWithAttributeTestCase()
    {
        var testType = TestSelector.GetTestType("Three");
        var testMethods = testType.GetMethodsWithAttribute<Case>();

        Assert.AreEqual(testMethods.Length, 4);
        Assert.IsTrue(testMethods.Any(t => t.Name == "Debug_Log"));
        Assert.IsTrue(testMethods.Any(t => t.Name == "IEnumerator_Log"));
    }

    [NUnit.Framework.Test]
    public void IsTestAssembly()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var findSelected = assemblies.Single(asm => asm.GetName().Name == "PerformanceTesting.Tests.Simple");

        Assert.IsTrue(TestSelector.IsTestAssembly(findSelected));
    }

    [NUnit.Framework.Test]
    public void GetTestCasesForValueSource()
    {
        var type = TestSelector.GetTestType("Four");
        var instance = Activator.CreateInstance(type);
        var testMethod = type.GetMethodsWithAttribute<Test>().Single(mi => mi.Name == "ValueSource");

        var cases = TestSelector.GetTestCasesForMethod(instance, type, testMethod);

        Assert.AreEqual(cases.Count, 6);
        Assert.IsTrue(cases.Any(t => t.Name == "ValueSource(VALUE1, VALUE1)"));
        Assert.IsTrue(cases.Any(t => t.Name == "ValueSource(VALUE1, VALUE2)"));
        Assert.IsTrue(cases.Any(t => t.Name == "ValueSource(VALUE1, VALUE3)"));
        Assert.IsTrue(cases.Any(t => t.Name == "ValueSource(VALUE2, VALUE1)"));
        Assert.IsTrue(cases.Any(t => t.Name == "ValueSource(VALUE2, VALUE2)"));
        Assert.IsTrue(cases.Any(t => t.Name == "ValueSource(VALUE2, VALUE3)"));
    }

    [NUnit.Framework.Test]
    public void GetTestCasesForTestCase()
    {
        var type = TestSelector.GetTestType("Four");
        var instance = Activator.CreateInstance(type);
        var testMethod = type.GetMethodsWithAttribute<Test>().Single(mi => mi.Name == "TestCaseMethod");

        var cases = TestSelector.GetTestCasesForMethod(instance, type, testMethod);

        Assert.AreEqual(cases.Count, 3);
        Assert.IsTrue(cases.Any(t => t.Name == "TestCaseMethod(FirstArg)"));
        Assert.IsTrue(cases.Any(t => t.Name == "TestCaseMethod(SecondArg)"));
        Assert.IsTrue(cases.Any(t => t.Name == "TestCaseMethod(ThirdArg)"));
    }

    [NUnit.Framework.Test]
    public void GetTestCasesForTest()
    {
        var type = TestSelector.GetTestType("Four");
        var instance = Activator.CreateInstance(type);
        var testMethod = type.GetMethodsWithAttribute<Test>().Single(mi => mi.Name == "SimpleTest");

        var cases = TestSelector.GetTestCasesForMethod(instance, type, testMethod);

        Assert.AreEqual(cases.Count, 1);
        Assert.IsTrue(cases.Any(t => t.Name == "SimpleTest()"));
    }

}
