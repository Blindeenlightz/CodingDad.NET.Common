using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingDad.Common.Loggers.DatabaseLoggers
{
	/// <summary>
	/// SQLite-specific logger implementation.
	/// </summary>
	public class SqliteLogger : DbLoggerBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SqliteLogger"/> class.
		/// </summary>
		/// <param name="connectionString">The SQLite database connection string.</param>
		/// <param name="logTable">The table name for storing logs.</param>
		/// <param name="minLogLevel">The minimum log level to log.</param>
		public SqliteLogger (string connectionString, string logTable, LogLevel minLogLevel)
			: base(connectionString, logTable, minLogLevel)
		{
		}

		/// <summary>
		/// Logs the message to the SQLite database.
		/// </summary>
		/// <param name="logLevel">The log level.</param>
		/// <param name="message">The log message.</param>
		/// <param name="exception">The exception to log.</param>
		protected override void LogToDatabase (LogLevel logLevel, string message, Exception exception)
		{
			try
			{
				using (var connection = new SqliteConnection(ConnectionString))
				{
					connection.Open();
					var command = connection.CreateCommand();
					command.CommandText = $@"
                        INSERT INTO {LogTable} (Timestamp, LogLevel, Message)
                        VALUES ($timestamp, $logLevel, $message);";

					command.Parameters.AddWithValue("$timestamp", DateTime.UtcNow);
					command.Parameters.AddWithValue("$logLevel", logLevel.ToString());
					command.Parameters.AddWithValue("$message", message);
					command.ExecuteNonQuery();
				}
			}
			catch (Exception ex)
			{
				// Handle logging failure (e.g., log to a fallback location)
				throw new Exception("An error occurred while logging to the SQLite database.", ex);
			}
		}
	}
}
