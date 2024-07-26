using CodingDad.Common.Loggers.ColorConsoleLogger;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace CodingDad.Common.Loggers
{
	public static class LoggerProvider
	{
		private static ILogger _logger;
		private static ColorConsoleLoggerProvider _provider = new ColorConsoleLoggerProvider();

		static LoggerProvider ()
		{
			_logger = _provider.CreateLogger(Assembly.GetExecutingAssembly().GetName().Name, LoggerOutputTarget.DebugWindow);
		}

		public static void Log (string message, LogLevel logLevel = LogLevel.Information)
		{
			_logger.Log(logLevel, message);
		}
	}
}