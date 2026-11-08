using System;
using UnityEngine;

namespace ValVertII.Core
{
    public enum AppLogLevel { Trace, Info, Warning, Error }

    public sealed class AppLogger : IAppService
    {
        public bool IsInitialized { get; private set; }
        public AppLogLevel MinimumLevel { get; set; } = AppLogLevel.Info;

        public void Initialize()
        {
            IsInitialized = true;
            Info("Logger initialisé.");
        }

        public void Trace(string message) => Write(AppLogLevel.Trace, message);
        public void Info(string message) => Write(AppLogLevel.Info, message);
        public void Warning(string message) => Write(AppLogLevel.Warning, message);

        public void Error(string message, Exception exception = null)
        {
            string full = exception == null ? message : $"{message}\n{exception}";
            Write(AppLogLevel.Error, full);
        }

        private void Write(AppLogLevel level, string message)
        {
            if (!IsInitialized && level != AppLogLevel.Error) return;
            if (level < MinimumLevel) return;

            string formatted = $"[ValVertII][{level}] {message}";

            switch (level)
            {
                case AppLogLevel.Warning:
                    Debug.LogWarning(formatted);
                    break;
                case AppLogLevel.Error:
                    Debug.LogError(formatted);
                    break;
                default:
                    Debug.Log(formatted);
                    break;
            }
        }

        public void Shutdown()
        {
            if (!IsInitialized) return;
            Info("Logger arrêté.");
            IsInitialized = false;
        }
    }
}