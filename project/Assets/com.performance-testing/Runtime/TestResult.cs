using System;
using System.Collections.Generic;

namespace PerformanceTesting
{
    [Serializable]
    public class TestResult
    {
        public List<SampleBuckets> SampleBuckets;
        public Queue<LogMessage> Logs;

        public TestResult()
        {
            Logs = new Queue<LogMessage>();
            SampleBuckets = new List<SampleBuckets>();
        }

        public void AddSample(string name, double value)
        {
            foreach (var bucket in SampleBuckets)
            {
                if (bucket.Name == name)
                {
                    bucket.Samples.Add(value);
                    return;
                }
            }

            var sb = new SampleBuckets(name);
            sb.Samples.Add(value);
            SampleBuckets.Add(sb);
        }
    }

    [Serializable]
    public class SampleBuckets
    {
        public SampleBuckets(string name)
        {
            Name = name;
        }

        public string Name;
        public List<double> Samples = new List<double>();
    }
}