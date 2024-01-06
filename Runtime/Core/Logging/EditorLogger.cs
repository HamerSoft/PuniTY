#define PUNITY_LOGGING //ToDo add to docs and configuration
using System;
using UnityEngine;

namespace HamerSoft.PuniTY.Core.Logging
{
    public class EditorLogger : ILogger
    {
        public void Log(string message)
        {
#if PUNITY_LOGGING
            Debug.Log(message);
#endif
        }

        public void LogWarning(string message)
        {
#if PUNITY_LOGGING
            Debug.LogWarning(message);
#endif
        }

        public void LogError(string message)
        {
#if PUNITY_LOGGING
            Debug.LogError(message);
#endif
        }

        public void LogError(string message, Exception exception)
        {
#if PUNITY_LOGGING
            Debug.LogError($"{message} | {exception} -> {exception.Message}");
#endif
        }
    }
}