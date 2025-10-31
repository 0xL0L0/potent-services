using System;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Potency.Services.Runtime.Utils.Logging
{
    public enum MLogLevel
    {
        Verbose,
        Info, 
        Warn,
        Error,
    }

    public static class PLog
    {
        private static readonly List<string> _currentDefineSymbols;
        
        [Conditional("LOG_LEVEL_VERBOSE")]
        public static void Verbose(string msgType, string msg)
        {
            TryOutputLog(GenerateMessage(msg, msgType), MLogLevel.Verbose);
        }
        
        [Conditional("LOG_LEVEL_VERBOSE")]
        public static void Verbose(string msg)
        {
            TryOutputLog(GenerateMessage(msg, ""), MLogLevel.Verbose);
        }
        
        [Conditional("LOG_LEVEL_VERBOSE")]
        [Conditional("LOG_LEVEL_INFO")]
        public static void Info(string msgType, string msg)
        {
            TryOutputLog(GenerateMessage(msg, msgType), MLogLevel.Info);
        }

        [Conditional("LOG_LEVEL_VERBOSE")]
        [Conditional("LOG_LEVEL_INFO")]
        public static void Info(string msg)
        {
            TryOutputLog(GenerateMessage(msg, ""), MLogLevel.Info);
        }
        
        [Conditional("LOG_LEVEL_VERBOSE")]
        [Conditional("LOG_LEVEL_INFO")]
        [Conditional("LOG_LEVEL_WARN")]
        public static void Warn(string msgType, string msg)
        {
            TryOutputLog(GenerateMessage(msg, msgType), MLogLevel.Warn);
        }
        
        [Conditional("LOG_LEVEL_VERBOSE")]
        [Conditional("LOG_LEVEL_INFO")]
        [Conditional("LOG_LEVEL_WARN")]
        public static void Warn(string msg)
        {
            TryOutputLog(GenerateMessage(msg, ""), MLogLevel.Warn);
        }
        
        [Conditional("LOG_LEVEL_VERBOSE")]
        [Conditional("LOG_LEVEL_INFO")]
        [Conditional("LOG_LEVEL_WARN")]
        [Conditional("LOG_LEVEL_ERROR")]
        public static void Error(string msgType, string msg)
        {
            TryOutputLog(GenerateMessage(msg, msgType), MLogLevel.Error);
        }
        
        [Conditional("LOG_LEVEL_VERBOSE")]
        [Conditional("LOG_LEVEL_INFO")]
        [Conditional("LOG_LEVEL_WARN")]
        [Conditional("LOG_LEVEL_ERROR")]
        public static void Error(string msg)
        {
            TryOutputLog(GenerateMessage(msg, ""), MLogLevel.Error);
        }

        private static void TryOutputLog(string msg, MLogLevel level)
        {
            var buildLogLevel = MLogLevel.Verbose;

#if LOG_LEVEL_ERROR
            buildLogLevel = MLogLevel.Error;
#endif

#if LOG_LEVEL_WARN
            buildLogLevel = MLogLevel.Warn;
#endif

#if LOG_LEVEL_INFO
            buildLogLevel = MLogLevel.Info;
#endif

#if LOG_LEVEL_VERBOSE
            buildLogLevel = MLogLevel.Verbose;
#endif

            // For debugging purposes, we want all logs on device. In editor, we can modify log level
#if UNITY_EDITOR
            if(level < buildLogLevel)
            {
                return;
            }
#endif
            switch(level)
            {
                case MLogLevel.Error:
                    Debug.LogError(msg);
                    break;

                case MLogLevel.Warn:
                    Debug.LogWarning(msg);
                    break;

                case MLogLevel.Info:
                case MLogLevel.Verbose:
                    Debug.Log(msg);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }

        private static string GenerateMessage(string msg, string msgType)
        {
            return $"[{msgType}]: {msg}";
        }
    }
}