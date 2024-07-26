using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;

namespace CodingDad.Common.Loggers.DatabaseLoggers
{
	public class SqlServerLogger : DbLoggerBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SqlServerLogger"/> class.
		/// </summary>
		/// <param name="connectionString">The SQL Server database connection string.</param>
		/// <param name="logTable">The table name for storing logs.</param>
		/// <param name="minLogLevel">The minimum log level to log.</param>
		public SqlServerLogger (string connectionString, string logTable, LogLevel minLogLevel)
			: base(connectionString, logTable, minLogLevel)
		{
		}

		/// <summary>
		/// Logs the message to the SQL Server database.
		/// </summary>
		protected override void LogToDatabase (LogLevel logLevel, string message, Exception exception)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(ConnectionString))
				{
					string query = $"INSERT INTO {LogTable} (Timestamp, LogLevel, Message) VALUES (@Timestamp, @LogLevel, @Message)";

					using (SqlCommand cmd = new SqlCommand(query, connection))
					{
						cmd.Parameters.AddWithValue("@Timestamp", DateTime.UtcNow);
						cmd.Parameters.AddWithValue("@LogLevel", logLevel.ToString());
						cmd.Parameters.AddWithValue("@Message", message);

						connection.Open();
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				// Handle logging failure (e.g., log to a fallback location)
				// Fallback logging logic can go here
				throw new Exception("An error occurred while logging to the SQL Server database.", ex);
			}
		}
	}
}