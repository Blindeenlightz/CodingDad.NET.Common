using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace CodingDad.Common.Loggers.DatabaseLoggers
{
	/// <summary>
	/// MongoDB-specific logger implementation.
	/// </summary>
	public class MongoDbLogger : DbLoggerBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MongoDbLogger"/> class.
		/// </summary>
		/// <param name="connectionString">The MongoDB database connection string.</param>
		/// <param name="logCollection">The collection name for storing logs.</param>
		/// <param name="minLogLevel">The minimum log level to log.</param>
		public MongoDbLogger (string connectionString, string logCollection, LogLevel minLogLevel)
			: base(connectionString, logCollection, minLogLevel)
		{
		}

		/// <summary>
		/// Logs the message to the MongoDB database.
		/// </summary>
		/// <param name="logLevel">The log level.</param>
		/// <param name="message">The log message.</param>
		/// <param name="exception">The exception to log.</param>
		protected override void LogToDatabase (LogLevel logLevel, string message, Exception exception)
		{
			try
			{
				var client = new MongoClient(ConnectionString);
				var database = client.GetDatabase(LogTable);
				var collection = database.GetCollection<BsonDocument>("logs");
				var log = new BsonDocument
				{
					{ "Timestamp", BsonDateTime.Create(DateTime.UtcNow) },
					{ "LogLevel", BsonString.Create(logLevel.ToString()) },
					{ "Message", BsonString.Create(message) }
				};
				collection.InsertOne(log);
			}
			catch (Exception ex)
			{
				// Handle logging failure (e.g., log to a fallback location)
				throw new Exception("An error occurred while logging to the MongoDB database.", ex);
			}
		}
	}
}