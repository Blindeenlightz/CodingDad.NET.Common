using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace CodingDad.Common.Loggers.ColorConsoleLogger
{
	/// <summary>
	/// Provides color-coded logging to the console.
	/// </summary>
	/// <remarks>
	/// LOGLEVEL ENUM
	/// -------------
	///	Trace   0
	/// Logs that contain the most detailed messages.These messages may contain sensitive application data.
	/// These messages are disabled by default and should never be enabled in a production environment.
	///
	///	Debug   1
	/// Logs that are used for interactive investigation during development.
	/// These logs should primarily contain information useful for debugging and have no long-term value.
	///
	///	Information 2
	/// Logs that track the general flow of the application.These logs should have long-term value.
	///
	///	Warning 3
	/// Logs that highlight an abnormal or unexpected event in the application flow,
	/// but do not otherwise cause the application execution to stop.
	///
	///	Error   4
	/// Logs that highlight when the current flow of execution is stopped due to a failure.
	/// These should indicate a failure in the current activity, not an application-wide failure.
	///
	/// Critical	5
	///	Logs that describe an unrecoverable application or system crash,
	///	or a catastrophic failure that requires immediate attention.
	///
	///	None    6
	/// Not used for writing log messages.Specifies that a logging category should not write any messages.
	///</remarks>

	public sealed class ColorConsoleLogger : ILogger
	{
		// Cache for thread-safe configuration fetching
		private static readonly ConcurrentDictionary<string, ColorConsoleLoggerConfiguration> ConfigCache = new();

		private readonly Func<ColorConsoleLoggerConfiguration> _getCurrentConfig;
		private readonly string _name;
		private readonly LoggerOutputTarget _outputTarget = LoggerOutputTarget.DebugWindow;

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorConsoleLogger"/> class.
		/// </summary>
		/// <param name="name">The name of the logger.</param>
		/// <param name="getCurrentConfig">A function to get the current logger configuration.</param>
		/// <param name="outputTarget">The output target for the logger. Optional, default is the debug output window.</param>
		public ColorConsoleLogger (
			string name,
			Func<ColorConsoleLoggerConfiguration> getCurrentConfig,
			LoggerOutputTarget outputTarget = LoggerOutputTarget.DebugWindow)
		{
			_name = name ?? throw new ArgumentNullException(nameof(name));
			_getCurrentConfig = getCurrentConfig ?? throw new ArgumentNullException(nameof(getCurrentConfig));
			_outputTarget = outputTarget;
		}

		/// <inheritdoc />
		/// <exception cref="NotSupportedException">The method is not supported.</exception>
		public IDisposable BeginScope<TState> (TState state) where TState : notnull
		{
			throw new NotSupportedException("BeginScope is not supported in ColorConsoleLogger.");
		}

		/// <inheritdoc />
		public bool IsEnabled (LogLevel logLevel) =>
			GetCurrentConfig().LogLevelToColorMap.ContainsKey(logLevel);

		/// <inheritdoc />
		public void Log<TState> (
			LogLevel logLevel,
			EventId eventId,
			TState state,
			Exception? exception,
			Func<TState, Exception?, string> formatter)
		{
			if (!IsEnabled(logLevel))
			{
				return;
			}

			var config = GetCurrentConfig();

			// Validate configuration
			if (!config.LogLevelToColorMap.ContainsKey(logLevel))
			{
				throw new InvalidOperationException($"No color mapping found for log level: {logLevel}");
			}

			// An event ID of 0 logs everything.
			// Otherwise we will only log if the event ID matches the config file.
			// This allows us to filter out specific events if we want to.
			if (config.EventId == 0 || config.EventId == eventId.Id)
			{
				WriteLog(logLevel, eventId, state, exception, formatter, config);
			}
		}

		private static void SetConsoleColor (ConsoleColor color)
		{
			// TODO: Add thread safety if required
			Console.ForegroundColor = color;
		}

		private ColorConsoleLoggerConfiguration GetCurrentConfig ()
		{
			// Use cached configuration if possible
			if (!ConfigCache.TryGetValue(_name, out var config))
			{
				config = _getCurrentConfig();
				ConfigCache [_name] = config;
			}
			return config;
		}

		private void WriteLog<TState> (
			LogLevel logLevel,
			EventId eventId,
			TState state,
			Exception? exception,
			Func<TState, Exception?, string> formatter,
			ColorConsoleLoggerConfiguration config)
		{
			if (_outputTarget == LoggerOutputTarget.Console)
			{
				ConsoleColor originalColor = Console.ForegroundColor;

				SetConsoleColor(config.LogLevelToColorMap [logLevel]);
				Console.WriteLine($"[{eventId.Id,2}: {logLevel,-12}]");

				SetConsoleColor(originalColor);
				Console.Write($"     {_name} - ");

				SetConsoleColor(config.LogLevelToColorMap [logLevel]);
				Console.Write($"{formatter(state, exception)}");

				SetConsoleColor(originalColor);
				Console.WriteLine();
			}
			else if (_outputTarget == LoggerOutputTarget.DebugWindow)
			{
				string formatWithEmoji (LogLevel level) => level switch
				{
					LogLevel.Trace => "🔍 Trace",
					LogLevel.Debug => "🐞 Debug",
					LogLevel.Information => "ℹ️ Information",
					LogLevel.Warning => "⚠️ Warning",
					LogLevel.Error => "❌ Error",
					LogLevel.Critical => "💥 Critical",
					_ => level.ToString()
				};

				Debug.WriteLine("");
				Debug.WriteLine(formatWithEmoji(logLevel));
				Debug.WriteLine($"[{eventId.Id}] {_name} - {formatter(state, exception)}");
				Debug.WriteLine("");
			}
		}
	}
}