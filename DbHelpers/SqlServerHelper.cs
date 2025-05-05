using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CodingDad.NET.Common.Loggers;
using Microsoft.Extensions.Logging;

namespace CodingDad.NET.Common.DbHelpers
{
    /// <summary>
    /// Provides a general-purpose interface for interacting with a SQL Server database.
    /// </summary>
    public class SqlServerHelper : IDatabaseHelper
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerHelper"/> class.
        /// </summary>
        /// <param name="connectionString">The SQL Server connection string.</param>
        public SqlServerHelper (string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <inheritdoc />
        public async Task<bool> CreateUserAsync (string email, string username, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                    string query = "INSERT INTO Users (Email, Username, Password) VALUES (@Email, @Username, @Password)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", hashedPassword);
                        await connection.OpenAsync();
                        int result = await command.ExecuteNonQueryAsync();
                        LoggerProvider.Log($"User created: {username}", LogLevel.Information);
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerProvider.Log($"An error occurred while creating the user: {ex.Message}", LogLevel.Error);
                return false;
            }
        }

        /// <summary>
        /// Executes a non-query SQL command.
        /// <code>
        /// // Example:
        /// string commandText = "INSERT INTO Users (Name, Age) VALUES (@Name, @Age)";
        /// SqlParameter[] parameters = {
        ///     new SqlParameter("@Name", "John"),
        ///     new SqlParameter("@Age", 30)
        /// };
        /// await sqlServerManager.ExecuteNonQueryAsync(commandText, parameters);
        /// </code>
        /// </summary>
        /// <param name="commandText">The SQL command text.</param>
        /// <param name="parameters">SQL parameters.</param>
        public async Task<int> ExecuteNonQueryAsync (string commandText, SqlParameter [] parameters = null)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        await connection.OpenAsync();
                        int result = await command.ExecuteNonQueryAsync();
                        LoggerProvider.Log($"Executed non-query: {commandText}", LogLevel.Information);
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerProvider.Log($"An error occurred while executing non-query: {ex.Message}", LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        /// Executes a SQL query and returns the result as a DataTable.
        /// <code>
        /// // Example:
        /// string queryText = "SELECT * FROM Users WHERE Age > @Age";
        /// SqlParameter[] parameters = {
        ///     new SqlParameter("@Age", 25)
        /// };
        /// DataTable result = await sqlServerManager.ExecuteQueryAsync(queryText, parameters);
        /// </code>
        /// </summary>
        /// <param name="queryText">The SQL query text.</param>
        /// <param name="parameters">SQL parameters.</param>
        /// <returns>A DataTable containing the query results.</returns>
        public async Task<DataTable> ExecuteQueryAsync (string queryText, SqlParameter [] parameters = null)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(queryText, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        await connection.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            LoggerProvider.Log($"Executed query: {queryText}", LogLevel.Information);
                            return dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerProvider.Log($"An error occurred while executing query: {ex.Message}", LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        /// Verifies if a user with the given email and password exists in the database.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>True if the user exists and the password is correct, false otherwise.</returns>
        public bool VerifyUser (string email, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT Password FROM Users WHERE Email = @Email";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            bool isVerified = BCrypt.Net.BCrypt.Verify(password, result.ToString());
                            LoggerProvider.Log($"User verification result for {email}: {isVerified}", LogLevel.Information);
                            return isVerified;
                        }
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerProvider.Log($"An error occurred while verifying the user: {ex.Message}", LogLevel.Error);
                return false;
            }
        }

        /// <inheritdoc />
        public async Task<bool> VerifyUserAsync (string email, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT Password FROM Users WHERE Email = @Email";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        if (result != null)
                        {
                            bool isVerified = BCrypt.Net.BCrypt.Verify(password, result.ToString());
                            LoggerProvider.Log($"User verification result for {email}: {isVerified}", LogLevel.Information);
                            return isVerified;
                        }
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerProvider.Log($"An error occurred while verifying the user: {ex.Message}", LogLevel.Error);
                return false;
            }
        }
    }
}
