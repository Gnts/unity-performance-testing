using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using PerformanceTesting;

namespace PerformanceTesting
{
    public static class ExtensionMethods
    {
        public static IEnumerator Invoke(this object instance, MethodInfo testMethod)
        {
            if (testMethod.ReturnType == typeof(IEnumerator))
                yield return testMethod.Invoke(instance, null);
            else
                testMethod.Invoke(instance, null);
        }

        public static IEnumerator Invoke(this object instance, MethodInfo testMethod, object[] args)
        {
            if (testMethod.ReturnType == typeof(IEnumerator))
                yield return testMethod.Invoke(instance, args);
            else
                testMethod.Invoke(instance, args);
        }

        public static IEnumerator Invoke(this object instance, IEnumerable<MethodInfo> testMethods)
        {
            foreach (var testMethod in testMethods)
            {
                yield return instance.Invoke(testMethod);
            }
        }

        public static T[] GetAttributes<T>(this MethodInfo testMethod) where T : Attribute
        {
            var attributes = testMethod.GetCustomAttributes(typeof(T), false);
            var filtered = new T[attributes.Length];
            for (int i = 0; i < attributes.Length; i++)
            {
                filtered[i] = (T)attributes[i];
            }
            return filtered;
        }

        public static T GetFirstAttribute<T>(this MethodInfo testMethod) where T : Attribute
        {
            var attributes = testMethod.GetCustomAttributes(typeof(T), false);
            if (attributes.Length == 0) return null;
            return (T)attributes[0];
        }

        public static T GetFirstAttribute<T>(this ParameterInfo parameterInfo) where T : Attribute
        {
            var attributes = parameterInfo.GetCustomAttributes(typeof(T), false);
            if (attributes.Length == 0) return null;
            return (T)attributes[0];
        }

        public static bool HasTests(this Type type)
        {
            var testMethods = type.GetMethodsWithAttribute<Test>().ToList();

            return testMethods.Any();
        }

        public static List<MethodInfo> GetTestMethods(this Type type)
        {
            var testMethods = type.GetMethodsWithAttribute<Test>().ToList();

            return testMethods;
        }
    }

}