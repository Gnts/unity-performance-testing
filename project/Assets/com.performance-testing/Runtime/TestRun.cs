using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using PerformanceTesting;
using UnityEngine;

namespace PerformanceTesting
{
    public static class TestRun
    {
        public static float Progress { get; private set; }
        public static Action<TestCase> TestStarted = delegate { };
        public static Action<TestCase> TestFinished = delegate { };
        public static Action RunStarted = delegate { };
        public static Action<List<TestPlan>> RunFinished = delegate { };

        public static IEnumerator Execute(params string[] filters)
        {
            RunStarted();

            var testTypes = TestSelector.GetAllTestTypes();
            List<TestPlan> plans = GetTestPlans(testTypes);

            Filter(plans, filters);

            for (int i = 0; i < plans.Count; i++)
            {
                Progress = (float)i / plans.Count;
                yield return plans[i].Execute();
            }

            yield return null;

            var result = new TestRunResult();
            result.Results = plans.SelectMany(p => p.TestCases).ToList();
            var json = JsonUtility.ToJson(result);
            var path = Application.streamingAssetsPath + "/PerformanceRunSettings.json";
            var settingsJson = File.ReadAllText(path);
            var settings = JsonUtility.FromJson<RunSettings>(settingsJson);
            Debug.Log("Saving results: " + settings.resultsPath + "/results.json");
            File.WriteAllText(settings.resultsPath + "/results.json", json);
            //Debug.Log("Saving results: " + Application.persistentDataPath + "/PerformanceRunResults.json");
            //File.WriteAllText(Application.persistentDataPath + "/PerformanceRunResults.json", json);
            RunFinished(plans);
        }

        public static List<TestPlan> GetTestPlans(List<Type> testTypes)
        {
            var plans = new List<TestPlan>();
            foreach (var testType in testTypes)
            {
                var item = new TestPlan(testType);
                plans.Add(item);
            }
            return plans;
        }

        public static void Filter(List<TestPlan> plans, params string[] filters)
        {
            if (filters.Length == 0) return;

            foreach (var plan in plans)
                plan.TestCases = plan.TestCases.Where(testCase => Array.Exists(filters, filter => testCase.Name.Contains(filter))).ToList();
        }
    }
}