using System;
using System.Collections;
using System.Reflection;
using PerformanceTesting;
using UnityEngine;

namespace PerformanceTesting
{
    [Serializable]
    public class TestCase
    {
        public static TestCase Active;
        public MethodInfo Method;
        public object[] Args;
        public string Name;
        public TestResult Result;

        public TestCase(MethodInfo method, object[] args)
        {
            this.Method = method;
            this.Args = args;
            this.Name = string.Format("{0}({1})", method.Name, Utils.CombineString(args));
            this.Result = new TestResult();
        }

        public IEnumerator Execute(object instance)
        {
            Active = this;
            Application.logMessageReceived += HandleLog;
            yield return instance.Invoke(Method, Args);
            Application.logMessageReceived -= HandleLog;
            Active = null;
        }

        void HandleLog(string logString, string stackTrace, LogType type)
        {
            Result.Logs.Enqueue(new LogMessage(logString, stackTrace, type));
        }
    }
}