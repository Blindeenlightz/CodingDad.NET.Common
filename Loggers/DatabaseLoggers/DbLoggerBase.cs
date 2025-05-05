using Microsoft.Extensions.Logging;
using System;

namespace CodingDad.NET.Common.Loggers.DatabaseLoggers
{
	/// <summary>
	/// Abstract base class for database loggers.
	/// </summary>
	public abstract class DbLoggerBase : ILogger
	{
		protected readonly string ConnectionString;
		protected readonly string LogTable;
		protected readonly LogLevel MinLogLevel;

		/// <summary>
		/// Initializes a new instance of the <see cref="DbLoggerBase"/> class.
		/// </summary>
		/// <param name="connectionString">The database connection string.</param>
		/// <param name="logTable">The table or collection name for storing logs.</param>
		/// <param name="minLogLevel">The minimum log level to log.</param>
		public DbLoggerBase (string connectionString, string logTable, LogLevel minLogLevel)
		{
			if (string.IsNullOrEmpty(connectionString))
			{
				throw new ArgumentException("Connection string must be provided.", nameof(connectionString));
			}

			if (string.IsNullOrEmpty(logTable))
			{
				throw new ArgumentException("Log table name must be provided.", nameof(logTable));
			}

			ConnectionString = connectionString;
			LogTable = logTable;
			MinLogLevel = minLogLevel;
		}

		/// <inheritdoc/>
		public IDisposable BeginScope<TState> (TState state) => null;

		/// <inheritdoc/>
		public bool IsEnabled (LogLevel logLevel)
		{
			return logLevel >= MinLogLevel;
		}

		/// <summary>
		/// Logs the specified log level, event ID, state, exception and formatter to the database.
		/// </summary>
		public void Log<TState> (LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			try
			{
				if (!IsEnabled(logLevel))
				{
					return;
				}

				string message = formatter(state, exception);
				LogToDatabase(logLevel, message, exception);
			}
			catch (Exception ex)
			{
				// Handle logging failure (e.g., log to a fallback location)
				// Fallback logging logic can go here
				throw new Exception("An error occurred while logging to the database.", ex);
			}
		}

		/// <summary>
		/// Abstract method to log the message to the database.
		/// </summary>
		protected abstract void LogToDatabase (LogLevel logLevel, string message, Exception exception);
	}
}