using System;

namespace HamerSoft.PuniTY.Logging
{
    public interface ILogger
    {
        public void Log(string message);
        public void LogWarning(string message);
        public void LogError(string message);
        public void LogError(string message, Exception exception);
    }
}