using UnityEngine;
using static Boopoo.Telemetry.Settings;

namespace Boopoo.Telemetry
{
    public static class LogHelper
    {
        private static string _logPrefix = "[Telemetry]";
        public static LoggingLevel LoggingLevel { get; set; } = LoggingLevel.Verbose;

        public static void LogVerbose(string message)
        {
            if (LoggingLevel <= LoggingLevel.Verbose) Debug.Log($"{_logPrefix} {message}");
        }

        public static void LogConnection(string message)
        {
            if (LoggingLevel <= LoggingLevel.Connections) Debug.Log($"{_logPrefix} {message}");
        }

        public static void LogWarning(string message)
        {
            if (LoggingLevel <= LoggingLevel.Warnings) Debug.LogWarning($"{_logPrefix} {message}");
        }

        public static void LogError(string message)
        {
            if (LoggingLevel <= LoggingLevel.Errors) Debug.LogError($"{_logPrefix} {message}");
        }

        public static void Disable()
        {
            LoggingLevel = LoggingLevel.Off;
        }
    }
}