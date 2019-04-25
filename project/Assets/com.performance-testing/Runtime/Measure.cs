using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace PerformanceTesting
{
    public static class Measure
    {
        public static ScopeMeasurement Scope(string name = "Time")
        {
            return new ScopeMeasurement(name);
        }

        public static IEnumerator Frametime(int count = 60, string name = "Time")
        {
            for (int i = 0; i < count; i++)
            {
                yield return null;
                Value(name, Time.unscaledDeltaTime);
            }
        }

        public static void Value(string name, double value)
        {
            TestCase.Active.Result.AddSample(name, value);
        }
    }

    public class ScopeMeasurement : IDisposable
    {
        Stopwatch sw;
        string name;

        public ScopeMeasurement(string name)
        {
            this.name = name;
            sw = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            sw.Stop();
            TestCase.Active.Result.AddSample(name, sw.Elapsed.TotalMilliseconds);
        }
    }
}