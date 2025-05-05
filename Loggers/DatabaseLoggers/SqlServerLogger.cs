using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using System;

namespace CodingDad.NET.Common.Loggers.DatabaseLoggers
{
    /// <summary>
    /// Provides a logging service using Microsoft.Data.SqlClient to write logs into a SQL Server table.
    /// </summary>
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
        /// Logs the message to the SQL Server database using Microsoft.Data.SqlClient.
        /// </summary>
        /// <param name="logLevel">The log level of the message.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">An optional exception associated with the log entry.</param>
        protected override void LogToDatabase (LogLevel logLevel, string message, Exception exception)
        {
            try
            {
                using var connection = new SqlConnection(ConnectionString);
                var query = $"INSERT INTO {LogTable} (Timestamp, LogLevel, Message) VALUES (@Timestamp, @LogLevel, @Message)";

                using var cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Timestamp", DateTime.UtcNow);
                cmd.Parameters.AddWithValue("@LogLevel", logLevel.ToString());
                cmd.Parameters.AddWithValue("@Message", message);

                connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while logging to the SQL Server database.", ex);
            }
        }
    }
}
