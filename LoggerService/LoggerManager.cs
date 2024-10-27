using Contracts;
using Serilog;
using Serilog.Core;

namespace LoggerService
{
    public class LoggerManager : ILoggerManager
    {
        private static readonly Logger Logger =
            new LoggerConfiguration().WriteTo.Console().CreateLogger();

        public void LogInfo(string message)
        {
            Logger.Information(message);
        }

        public void LogWarn(string message)
        {
            Logger.Warning(message);
        }

        public void LogDebug(string message)
        {
            Logger.Debug(message);
        }

        public void LogError(string message)
        {
            Logger.Error(message);
        }
    }
}