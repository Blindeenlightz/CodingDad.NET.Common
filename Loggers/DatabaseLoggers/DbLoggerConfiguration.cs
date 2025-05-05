using Microsoft.Extensions.Logging;

namespace CodingDad.NET.Common.Loggers.DatabaseLoggers
{
	/// <summary>
	/// Base configuration class for database loggers.
	/// </summary>
	public class DbLoggerConfiguration
	{
		/// <summary>
		/// Gets or sets the connection string for the database.
		/// </summary>
		public string ConnectionString { get; set; }

		/// <summary>
		/// Gets or sets the database type (e.g., "SqlServer", "MongoDb", "SqlLite").
		/// </summary>
		public string DatabaseType { get; set; }

		/// <summary>
		/// Gets or sets the table or collection name for storing logs.
		/// </summary>
		public string LogTable { get; set; }

		/// <summary>
		/// Gets or sets the minimum log level to log.
		/// </summary>
		public LogLevel MinLogLevel { get; set; } = LogLevel.Information;
	}

	/// <summary>
	/// Configuration class for MongoDB logger.
	/// </summary>
	public class MongoDbLoggerConfiguration : DbLoggerConfiguration
	{
		// Add any MongoDB-specific settings here
	}

	/// <summary>
	/// Configuration class for SQLite logger.
	/// </summary>
	public class SqlLiteLoggerConfiguration : DbLoggerConfiguration
	{
		// Add any SQLite-specific settings here
	}

	/// <summary>
	/// Configuration class for SQL Server logger.
	/// </summary>
	public class SqlLoggerConfiguration : DbLoggerConfiguration
	{
		// Add any SQL Server-specific settings here
	}
}