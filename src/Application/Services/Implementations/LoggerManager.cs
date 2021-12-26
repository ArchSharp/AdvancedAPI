using System;
using NLog;
using ShareLoanApp.Application.Services.Interfaces;

namespace Application.Services.Implementations
{
    public class LoggerManager: ILoggerManager
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public void LogInfo(string message)
        {
            Logger.Info(message);
        }

        public void LogWarn(string message)
        {
            Logger.Warn(message);
        }

        public void LogDebug(string message)
        {
            Logger.Debug(message);
        }

        public void LogError(Exception ex)
        {
            Logger.Error(ex);
        }
    }
}