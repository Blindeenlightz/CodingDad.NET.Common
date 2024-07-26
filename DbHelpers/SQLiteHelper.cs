using CodingDad.Common.Loggers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CodingDad.Common.DbHelpers
{
	/// <summary>
	/// Provides a general-purpose interface for interacting with a SQLite database.
	/// </summary>
	public class SQLiteHelper : IDatabaseHelper
	{
		private readonly string _connectionString;

		/// <summary>
		/// Initializes a new instance of the <see cref="SQLiteHelper"/> class.
		/// </summary>
		/// <param name="connectionString">The SQLite connection string.</param>
		public SQLiteHelper (string connectionString)
		{
			_connectionString = connectionString;
		}

		/// <summary>
		/// Asynchronously creates a table if it doesn't already exist.
		/// </summary>
		/// <param name="createTableQuery">The SQL query for creating the table.</param>
		/// <returns>A Task representing the asynchronous operation.</returns>
		/// <exception cref="InvalidOperationException">Thrown when the table creation fails.</exception>
		public virtual async Task CreateTableAsync (string createTableQuery)
		{
			await ValidateDatabaseFileExistsAsync().ConfigureAwait(false);

			try
			{
				await ExecuteNonQueryAsync(createTableQuery).ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				// TODO: Implement proper logging
				throw new InvalidOperationException("Failed to create table.", ex);
			}
		}

		/// <inheritdoc />
		public async Task<bool> CreateUserAsync (string email, string username, string password)
		{
			await ValidateDatabaseFileExistsAsync().ConfigureAwait(false);

			if (string.IsNullOrEmpty(email))
			{
				throw new ArgumentException("Email must not be null or empty.", nameof(email));
			}

			if (string.IsNullOrEmpty(username))
			{
				throw new ArgumentException("Username must not be null or empty.", nameof(username));
			}

			if (string.IsNullOrEmpty(password))
			{
				throw new ArgumentException("Password must not be null or empty.", nameof(password));
			}

			try
			{
				string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
				string query = "INSERT INTO Users (Email, Username, Password) VALUES (@Email, @Username, @Password)";
				SQLiteParameter [] parameters = {
			new SQLiteParameter("@Email", email),
			new SQLiteParameter("@Username", username),
			new SQLiteParameter("@Password", hashedPassword)
		};
				int result = await ExecuteNonQueryAsync(query, parameters).ConfigureAwait(false);
				return result > 0;
			}
			catch (SQLiteException ex)
			{
				// TODO: Log the exception
				throw new SQLiteException("Failed to create user.", ex);
			}
		}

		/// <summary>
		/// Deletes a record from a specified table based on a condition.
		/// </summary>
		/// <param name="tableName">The name of the table from which to delete the record.</param>
		/// <param name="conditionColumn">The column name to be used in the WHERE clause for deletion.</param>
		/// <param name="conditionValue">The value to be matched against the conditionColumn for deletion.</param>
		/// <returns>A <see cref="Task"/> representing the asynchronous operation, with a boolean result indicating success or failure.</returns>
		/// <exception cref="ArgumentException">Thrown when the table name or condition column is null or empty.</exception>
		/// <exception cref="SQLiteException">Thrown when the database operation fails.</exception>
		public virtual async Task<bool> DeleteRecordAsync (string tableName, string conditionColumn, object conditionValue)
		{
			await ValidateDatabaseFileExistsAsync().ConfigureAwait(false);

			if (string.IsNullOrEmpty(tableName))
			{
				throw new ArgumentException("Table name must not be null or empty.", nameof(tableName));
			}

			if (string.IsNullOrEmpty(conditionColumn))
			{
				throw new ArgumentException("Condition column must not be null or empty.", nameof(conditionColumn));
			}

			if (conditionValue == null)
			{
				throw new ArgumentNullException(nameof(conditionValue), "Condition value must not be null.");
			}

			try
			{
				string query = $"DELETE FROM {tableName} WHERE {conditionColumn} = @ConditionValue";
				SQLiteParameter [] parameters =
				{
					new SQLiteParameter("@ConditionValue", conditionValue)
				};
				int result = await ExecuteNonQueryAsync(query, parameters).ConfigureAwait(false);
				return result > 0;
			}
			catch (SQLiteException ex)
			{
				// TODO: Log the exception
				throw new SQLiteException($"Failed to delete record from table {tableName}.", ex);
			}
		}

		/// <summary>
		/// Executes a non-query SQL command asynchronously.
		/// </summary>
		/// <param name="commandText">The SQL command text.</param>
		/// <param name="parameters">SQLite parameters.</param>
		/// <returns>A Task<int> representing the number of rows affected.</returns>
		/// <exception cref="Exception">Thrown when the query execution fails.</exception>
		public virtual async Task<int> ExecuteNonQueryAsync (string commandText, SQLiteParameter [] parameters = null)
		{
			await ValidateDatabaseFileExistsAsync().ConfigureAwait(false);
			LoggerProvider.Log($"Executing SQL command: {commandText} with parameters: {string.Join(",", parameters?.Select(p => p.ToString()) ?? new List<string>())}", LogLevel.Debug);

			try
			{
				using (var connection = new SQLiteConnection(_connectionString))
				{
					LoggerProvider.Log($"Attempting to open connection to database with connection string: {_connectionString}", LogLevel.Debug);

					await connection.OpenAsync().ConfigureAwait(false);

					LoggerProvider.Log($"Successfully opened connection to database.", LogLevel.Debug);

					using (var command = new SQLiteCommand(commandText, connection))
					{
						if (parameters != null)
						{
							command.Parameters.AddRange(parameters);
						}

						int rowsAffected = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
						LoggerProvider.Log($"SQL command affected {rowsAffected} rows.", LogLevel.Debug);
						return rowsAffected;
					}
				}
			}
			catch (SQLiteException ex)
			{
				LoggerProvider.Log($"An SQLite error occurred while executing non-query: {ex.Message}. SQLite error code: {ex.ResultCode}", LogLevel.Error);

				throw new Exception($"An error occurred while executing non-query: {ex.Message}", ex);
			}
			catch (Exception ex)
			{
				LoggerProvider.Log($"An error occurred while executing non-query: {ex.Message}", LogLevel.Error);
				throw new Exception($"An error occurred while executing non-query: {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Executes a SQL query and returns the result as a DataTable.
		/// </summary>
		/// <param name="queryText">The SQL query text.</param>
		/// <param name="parameters">SQLite parameters.</param>
		/// <returns>A Task<DataTable> containing a DataTable with query results.</returns>
		/// <exception cref="Exception">Thrown when the query execution fails.</exception>
		public virtual async Task<DataTable> ExecuteQueryAsync (string queryText, SQLiteParameter [] parameters = null)
		{
			LoggerProvider.Log("Starting ExecuteQueryAsync method.", LogLevel.Debug);

			// Validate database file existence
			await ValidateDatabaseFileExistsAsync().ConfigureAwait(false);

			try
			{
				using (var connection = new SQLiteConnection(_connectionString))
				{
					LoggerProvider.Log("Attempting to open database connection.", LogLevel.Debug);
					await connection.OpenAsync().ConfigureAwait(false);
					LoggerProvider.Log("Successfully opened connection to database.", LogLevel.Debug);

					using (var command = new SQLiteCommand(queryText, connection))
					{
						LoggerProvider.Log($"Executing SQL query: {queryText}", LogLevel.Information);

						if (parameters != null)
						{
							command.Parameters.AddRange(parameters);
							LoggerProvider.Log($"Added {parameters.Length} SQL parameters.", LogLevel.Debug);
						}

						using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
						{
							LoggerProvider.Log("Successfully executed SQL query, loading results into DataTable.", LogLevel.Debug);
							var dataTable = new DataTable();
							dataTable.Load(reader);
							LoggerProvider.Log("Successfully loaded query results into DataTable.", LogLevel.Information);
							return dataTable;
						}
					}
				}
			}
			catch (SQLiteException ex)
			{
				LoggerProvider.Log($"SQLite error occurred while executing query: {ex.Message}. SQLite error code: {ex.ResultCode}", LogLevel.Error);
				throw new Exception($"An SQLite error occurred while executing query: {ex.Message}", ex);
			}
			catch (Exception ex)
			{
				LoggerProvider.Log($"An error occurred while executing query: {ex.Message}", LogLevel.Error);
				throw new Exception($"An error occurred while executing query: {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Inserts a new record into a specified table.
		/// </summary>
		/// <param name="tableName">The name of the table into which to insert the new record.</param>
		/// <param name="columnData">A dictionary containing the column names and their corresponding values for the new record.</param>
		/// <returns>A <see cref="Task"/> representing the asynchronous operation, with a boolean result indicating success or failure.</returns>
		/// <exception cref="ArgumentException">Thrown when the table name is null or empty, or when the column data is null or empty.</exception>
		/// <exception cref="SQLiteException">Thrown when the database operation fails.</exception>
		public virtual async Task<bool> InsertIntoTableAsync (string tableName, Dictionary<string, object?> columnData)
		{
			await ValidateDatabaseFileExistsAsync().ConfigureAwait(false);

			if (string.IsNullOrEmpty(tableName))
			{
				throw new ArgumentException("Table name must not be null or empty.", nameof(tableName));
			}

			if (columnData == null || columnData.Count == 0)
			{
				throw new ArgumentException("Column data must not be null or empty.", nameof(columnData));
			}

			try
			{
				string columns = string.Join(", ", columnData.Keys);
				string parameters = string.Join(", ", columnData.Keys.Select(k => "@" + k));
				string query = $"INSERT INTO {tableName} ({columns}) VALUES ({parameters})";

				List<SQLiteParameter> sqlParameters = new List<SQLiteParameter>();
				foreach (var kvp in columnData)
				{
					sqlParameters.Add(new SQLiteParameter("@" + kvp.Key, kvp.Value ?? DBNull.Value));
				}

				int result = await ExecuteNonQueryAsync(query, sqlParameters.ToArray()).ConfigureAwait(false);
				return result > 0;
			}
			catch (Exception ex)
			{
				// TODO: Log the exception
				throw new SQLiteException($"Failed to insert record into table {tableName}.", ex);
			}
		}

		/// <summary>
		/// Asynchronously retrieves the serialized states of all instances of a specific ViewModel from the database.
		/// </summary>
		/// <param name="viewModelName">The name of the ViewModel whose states are being retrieved.</param>
		/// <returns>A <see cref="Task"/> representing the asynchronous operation, containing a list of serialized states.</returns>
		/// <exception cref="ArgumentException">Thrown when <paramref name="viewModelName"/> is null or empty.</exception>
		/// <exception cref="SQLiteException">Thrown when database operation fails.</exception>
		public virtual async Task<List<string>> RetrieveAllViewModelStatesAsync (string viewModelName)
		{
			await ValidateDatabaseFileExistsAsync().ConfigureAwait(false);

			if (string.IsNullOrEmpty(viewModelName))
			{
				throw new ArgumentException("ViewModel name must not be null or empty.", nameof(viewModelName));
			}

			try
			{
				string query = "SELECT State FROM ViewModelStates WHERE Name = @Name";
				SQLiteParameter [] parameters = {
					new SQLiteParameter("@Name", viewModelName)
				};

				DataTable result = await ExecuteQueryAsync(query, parameters).ConfigureAwait(false);

				List<string> serializedStates = new List<string>();
				foreach (DataRow row in result.Rows)
				{
					serializedStates.Add(row ["State"].ToString());
				}

				return serializedStates;
			}
			catch (Exception ex)
			{
				// TODO: Log the exception
				throw new SQLiteException("Failed to retrieve ViewModel states.", ex);
			}
		}

		/// <summary>
		/// Asynchronously saves the serialized state of a ViewModel into the database.
		/// </summary>
		/// <param name="viewModelName">The name of the ViewModel whose state is being saved. Must not be null or empty.</param>
		/// <param name="serializedState">The serialized state of the ViewModel. Must not be null.</param>
		/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
		/// <exception cref="ArgumentException">Thrown when <paramref name="viewModelName"/> is null or empty.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="serializedState"/> is null.</exception>
		/// <exception cref="SQLiteException">Thrown when database operation fails.</exception>
		public virtual async Task SaveSingletonViewModelStateAsync (string viewModelName, string serializedState)
		{
			await ValidateDatabaseFileExistsAsync().ConfigureAwait(false);

			if (string.IsNullOrEmpty(viewModelName))
			{
				throw new ArgumentException("ViewModel name must not be null or empty.", nameof(viewModelName));
			}

			if (serializedState is null)
			{
				throw new ArgumentNullException(nameof(serializedState), "Serialized state must not be null.");
			}

			try
			{
				string query = "INSERT OR REPLACE INTO ViewModelStates (Name, State) VALUES (@Name, @State)";
				SQLiteParameter [] parameters = {
					new SQLiteParameter("@Name", viewModelName),
					new SQLiteParameter("@State", serializedState)
				};
				await ExecuteNonQueryAsync(query, parameters).ConfigureAwait(false);
			}
			catch (SQLiteException ex)
			{
				// TODO: Log the exception
				throw new SQLiteException("Failed to save ViewModel state.", ex);
			}
		}

		public virtual async Task SaveViewModelStateAsync (string viewModelName, string instanceId, string serializedState)
		{
			await ValidateDatabaseFileExistsAsync().ConfigureAwait(false);

			if (string.IsNullOrEmpty(viewModelName))
			{
				throw new ArgumentException("ViewModel name must not be null or empty.", nameof(viewModelName));
			}

			if (string.IsNullOrEmpty(instanceId))
			{
				throw new ArgumentException("Instance ID must not be null or empty.", nameof(instanceId));
			}

			if (serializedState is null)
			{
				throw new ArgumentNullException(nameof(serializedState), "Serialized state must not be null.");
			}

			try
			{
				string query = "INSERT OR REPLACE INTO ViewModelStates (Name, InstanceId, State) VALUES (@Name, @InstanceId, @State)";
				SQLiteParameter [] parameters = {
					new SQLiteParameter("@Name", viewModelName),
					new SQLiteParameter("@InstanceId", instanceId),
					new SQLiteParameter("@State", serializedState)
				};
				await ExecuteNonQueryAsync(query, parameters).ConfigureAwait(false);
			}
			catch (SQLiteException ex)
			{
				// TODO: Log the exception
				throw new SQLiteException("Failed to save ViewModel state.", ex);
			}
		}

		/// <summary>
		/// Stores a unique identifier for a user.
		/// </summary>
		/// <param name="userId">The user ID to which the unique identifier will be linked.</param>
		/// <param name="uniqueIdentifier">The unique identifier to store.</param>
		/// <returns>True if the operation was successful; otherwise, false.</returns>
		public virtual async Task<bool> StoreUniqueIdentifierAsync (string userId, string uniqueIdentifier)
		{
			await ValidateDatabaseFileExistsAsync().ConfigureAwait(false);

			if (string.IsNullOrEmpty(userId))
			{
				throw new ArgumentException("User ID must not be null or empty.", nameof(userId));
			}

			if (string.IsNullOrEmpty(uniqueIdentifier))
			{
				throw new ArgumentException("Unique Identifier must not be null or empty.", nameof(uniqueIdentifier));
			}

			try
			{
				string query = "INSERT INTO UserIdentifiers (UserId, UniqueIdentifier) VALUES (@UserId, @UniqueIdentifier)";
				SQLiteParameter [] parameters = {
			new SQLiteParameter("@UserId", userId),
			new SQLiteParameter("@UniqueIdentifier", uniqueIdentifier)
		};
				int result = await ExecuteNonQueryAsync(query, parameters).ConfigureAwait(false);
				return result > 0;
			}
			catch (SQLiteException ex)
			{
				// TODO: Log the exception
				throw new SQLiteException("Failed to store unique identifier.", ex);
			}
		}

		/// <inheritdoc />
		public async Task<bool> VerifyUserAsync (string email, string password)
		{
			await ValidateDatabaseFileExistsAsync().ConfigureAwait(false);

			if (string.IsNullOrEmpty(email))
			{
				throw new ArgumentException("Email must not be null or empty.", nameof(email));
			}

			if (string.IsNullOrEmpty(password))
			{
				throw new ArgumentException("Password must not be null or empty.", nameof(password));
			}

			try
			{
				string query = "SELECT Password FROM Users WHERE Email = @Email";
				SQLiteParameter [] parameters =
				{
					new SQLiteParameter("@Email", email)
				};
				object result = await ExecuteQueryAsync(query, parameters).ConfigureAwait(false);
				return result != null && BCrypt.Net.BCrypt.Verify(password, result.ToString());
			}
			catch (SQLiteException ex)
			{
				// TODO: Log the exception
				throw new SQLiteException("Failed to verify user.", ex);
			}
		}

		/// <summary>
		/// Validates that the SQLite database file exists.
		/// </summary>
		/// <returns>A Task representing the asynchronous operation.</returns>
		/// <exception cref="FileNotFoundException">Thrown if the database file is not found.</exception>
		private async Task ValidateDatabaseFileExistsAsync ()
		{
			var dbPath = new SQLiteConnectionStringBuilder(_connectionString).DataSource;
			if (!File.Exists(dbPath))
			{
				throw new FileNotFoundException($"Database file not found at {dbPath}");
			}
		}
	}
}