using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace CodingDad.NET.Common.Loggers.ColorConsoleLogger
{
	/// <summary>
	/// Represents the configuration settings for the ColorConsoleLogger.
	/// </summary>
	public sealed class ColorConsoleLoggerConfiguration
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ColorConsoleLoggerConfiguration"/> class.
		/// </summary>
		/// <param name="eventId">The EventId for logging.</param>
		/// <param name="logLevelToColorMap">The mapping between LogLevel and ConsoleColors.</param>
		public ColorConsoleLoggerConfiguration (int eventId, Dictionary<LogLevel, ConsoleColor> logLevelToColorMap)
		{
			EventId = eventId;
			LogLevelToColorMap = new Dictionary<LogLevel, ConsoleColor>(logLevelToColorMap);
		}

		/// <summary>
		/// Gets the EventId for logging.
		/// </summary>
		public int EventId { get; }

		public LoggerOutputTarget OutputTarget { get; set; } = LoggerOutputTarget.Console;

		/// <summary>
		/// Gets the mapping between LogLevel and ConsoleColors.
		/// </summary>
		public IReadOnlyDictionary<LogLevel, ConsoleColor> LogLevelToColorMap { get; private set; }
	}
}