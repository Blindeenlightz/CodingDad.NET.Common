using CodingDad.Common.Constants;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;

namespace CodingDad.Common.Loggers.DatabaseLoggers
{
	/// <summary>
	/// Provides instances of <see cref="DbLoggerBase"/> based on configuration.
	/// </summary>
	public class DbLoggerProvider : ILoggerProvider
	{
		private readonly DbLoggerConfiguration _config;
		private readonly ConcurrentDictionary<string, DbLoggerBase> _loggers = new ConcurrentDictionary<string, DbLoggerBase>();

		/// <summary>
		/// Initializes a new instance of the <see cref="DbLoggerProvider"/> class.
		/// </summary>
		/// <param name="config">The logger configuration.</param>
		public DbLoggerProvider (DbLoggerConfiguration config)
		{
			_config = config ?? throw new ArgumentNullException(nameof(config));
		}

		/// <summary>
		/// Creates a new logger instance of the specified category.
		/// </summary>
		public ILogger CreateLogger (string categoryName)
		{
			return _loggers.GetOrAdd(categoryName, name => CreateLoggerInstance());
		}

		/// <summary>
		/// Disposes the logger provider and releases resources.
		/// </summary>
		public void Dispose ()
		{
			// Implement disposal logic if needed
		}

		/// <summary>
		/// Creates a logger instance based on the current configuration.
		/// </summary>
		private DbLoggerBase CreateLoggerInstance ()
		{
			switch (_config.DatabaseType)
			{
				case DatabaseTypes.SqlServer:
					return new SqlServerLogger(_config.ConnectionString, _config.LogTable, _config.MinLogLevel);
				case DatabaseTypes.Sqlite:
					return new SqliteLogger(_config.ConnectionString, _config.LogTable, _config.MinLogLevel);
				case DatabaseTypes.MongoDb:
					return new MongoDbLogger(_config.ConnectionString, _config.LogTable, _config.MinLogLevel);
				default:
					throw new NotSupportedException($"Database type {_config.DatabaseType} is not supported.");
			}
		}
	}
}