using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PerformanceTesting;
using UnityEngine;

namespace PerformanceTesting
{
    public static class TestSelector
    {
        public static List<TestCase> GetTestCasesForMethod(object instance, Type TestType, MethodInfo testMethod)
        {
            List<TestCase> testCases = new List<TestCase>();

            var testCaseAttributes = testMethod.GetAttributes<Case>();
            foreach (var testCase in testCaseAttributes)
                testCases.Add(new TestCase(testMethod, testCase.args));

            var parameters = testMethod.GetParameters();
            MethodInfo[] valueSourceMethods = parameters
                .Where(a => a.GetFirstAttribute<ValueSource>() != null)
                .Select(a => a.GetFirstAttribute<ValueSource>())
                .Select(a => TestType.GetMethod(a.Method)).ToArray();
            var list = TestSelector.GetValueSourceArguments(instance, valueSourceMethods);
            if (list.Count > 0)
            {
                List<object[]> cases = TestSelector.CombineTestCases(list);
                foreach (var testCase in cases)
                    testCases.Add(new TestCase(testMethod, testCase));
            }

            if (parameters.Length == 0)
                testCases.Add(new TestCase(testMethod, null));

            return testCases;
        }

        public static List<object[]> GetValueSourceArguments(object instance, MethodInfo[] valueSourceMethods)
        {
            List<object[]> list = new List<object[]>();
            foreach (var item in valueSourceMethods)
                if (item == null)
                    throw new MissingMethodException("ValueSource attribute is missing method implementation.");

            var argArray = valueSourceMethods.Select(valueSourceMethod => valueSourceMethod.Invoke(instance, null));
            foreach (var o in argArray)
            {
                var objArray = o as Array;
                var arrayOfObjs = new object[objArray.Length];
                for (int i = 0; i < objArray.Length; i++)
                {
                    arrayOfObjs[i] = objArray.GetValue(i);
                }

                list.Add(arrayOfObjs);
            }

            return list;
        }

        public static List<object[]> CombineTestCases(List<object[]> list)
        {
            List<object[]> cases = new List<object[]>();
            List<int> state = new List<int>();
            for (int i = 0; i < list.Count; i++)
                state.Add(0);


            while (true)
            {
                // produce element
                var element = new object[list.Count];

                for (int i = 0; i < list.Count; i++)
                {
                    element[i] = list[i][state[i]];
                }

                cases.Add(element);

                // advance to the next state
                for (int i = 0; i < list.Count; i++)
                {
                    state[i]++;
                    if (state[i] < list[i].Length)
                        break;

                    state[i] = 0;
                }

                // If we get back to a start state, exit
                bool done = true;
                for (int i = 0; i < list.Count; i++)
                {
                    if (state[i] != 0)
                    {
                        done = false;
                        break;
                    }
                }

                if (done)
                    break;
            }

            return cases;
        }

        public static List<Assembly> GetAllTestAssemblies()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var testAssemblies = assemblies.Where(asm =>
                asm.GetReferencedAssemblies().Any(asmRef => asmRef.Name.Contains(Constants.TestAssemblyName))).ToList();
            return testAssemblies;
        }

        public static bool IsTestAssembly(Assembly asm)
        {
            if (asm == null) return false;
            return asm.GetReferencedAssemblies().Any(asmRef => asmRef.Name.Contains(Constants.TestAssemblyName));
        }

        public static List<Type> GetAllTestTypes()
        {
            return GetAllTestAssemblies().SelectMany(asm => asm.GetTypes().Where(
                type => type.HasTests())).ToList();
        }

        public static Assembly GetTestAssembly(string assemblyName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var findSelected = assemblies.Single(asm => asm.GetName().Name == assemblyName);

            if (IsTestAssembly(findSelected)) return findSelected;
            return null;
        }

        public static List<Assembly> GetTestAssemblies(params string[] assemblyNames)
        {
            if(assemblyNames.Length == 0) throw new ArgumentNullException("Provide at least one assembly name.");
            var assemblies = new List<Assembly>();

            foreach (var asm in assemblyNames)
                assemblies.Add(GetTestAssembly(asm));

            return assemblies;
        }

        public static Type GetTestType(string type)
        {
            var types = GetAllTestTypes();
            return types.Single(t => t.Name == type);
        }

        public static List<Type> GetTestTypes(params string[] types)
        {
            if(types.Length == 0) throw new ArgumentNullException("Provide at least one type.");
            var allTestTypes = GetAllTestTypes();
            return allTestTypes.Where(t => types.Contains(t.Name)).ToList();
        }

        public static List<Type> GetTestTypesFromAssembly(string assembly)
        {
            Assembly asm = GetTestAssembly(assembly);
            return asm.GetTypes().Where(type => type.HasTests()).ToList();
        }

        public static List<Type> GetTestTypesFromAssemblies(params string[] assemblyNames)
        {
            if(assemblyNames.Length == 0) throw new ArgumentNullException("Provide at least one assembly name.");
            var types = new List<Type>();

            foreach (var asm in assemblyNames)
                types.AddRange(GetTestTypesFromAssembly(asm));

            return types;
        }

        public static MethodInfo[] GetMethodsWithAttribute<T>(this Type type) where T : Attribute
        {
            var testMethods = type.GetMethods()
                .Where(mi => mi.GetCustomAttributes(typeof(T), false).Length > 0).ToArray();

            return testMethods;
        }

        // @TODO - tests
        public static MethodInfo[] GetPrebuildMethods(this Type type)
        {
            var testMethods = type.GetMethods()
                .Where(mi => mi.GetFirstAttribute<PrebuildSetup>() != null)
                .Where(mi => mi.GetFirstAttribute<PrebuildSetup>().TypeName == "null")
                .ToArray();

            return testMethods;
        }

        // @TODO - tests
        public static string[] GetPrebuildTypeNames(this Type type)
        {
            var testMethods = type.GetMethods()
                .Where(mi => mi.GetFirstAttribute<PrebuildSetup>() != null)
                .Select(mi => mi.GetFirstAttribute<PrebuildSetup>().TypeName)
                .Where(typeName => typeName != "null")
                .ToArray();

            return testMethods;
        }
    }
}