using UnityEngine;

namespace PerformanceTesting
{
    public struct LogMessage
    {
        public string log;
        public string stackTrace;
        public LogType type;

        public LogMessage(string log, string stackTrace, LogType type)
        {
            this.log = log;
            this.stackTrace = stackTrace;
            this.type = type;
        }
    }
}