using CodingDad.NET.Common.Loggers.ColorConsoleLogger;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace CodingDad.NET.Common.Loggers
{
    /// <summary>
    /// Provides a static logging service using the ColorConsoleLoggerProvider.
    /// </summary>
    public static class LoggerProvider
    {
        private static ILogger _logger;
        private static ColorConsoleLoggerProvider _provider = new ColorConsoleLoggerProvider();

        /// <summary>
        /// Static constructor to initialize the logger.
        /// </summary>
        static LoggerProvider ()
        {
            _logger = _provider.CreateLogger(Assembly.GetExecutingAssembly().GetName().Name, LoggerOutputTarget.DebugWindow);
        }

        /// <summary>
        /// Logs a message with the specified log level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="logLevel">The log level (default is Information).</param>
        public static void Log (string message, LogLevel logLevel = LogLevel.Information)
        {
            _logger.Log(logLevel, message);
        }
    }
}
