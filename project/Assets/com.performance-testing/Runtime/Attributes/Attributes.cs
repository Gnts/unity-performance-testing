using System;

namespace PerformanceTesting
{
    [AttributeUsage(AttributeTargets.Method)]
    public class OneTimeSetup : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class OneTimeTeardown : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class Setup : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class Teardown : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class Test : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Method)]
    public class PrebuildSetup : Attribute
    {
        public string TypeName;

        public PrebuildSetup()
        {
            this.TypeName = "null";
        }

        public PrebuildSetup(string typeNameName)
        {
            this.TypeName = typeNameName;
        }
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public class ValueSource : Attribute
    {
        public string Method;

        public ValueSource(string method)
        {
            this.Method = method;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class Case : Attribute
    {
        public object[] args;

        public Case(params object[] args)
        {
            this.args = args;
        }
    }
}