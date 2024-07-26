using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodingDad.Common.Loggers;
using Microsoft.Extensions.Logging;

namespace CodingDad.Common.DbHelpers
{
    /// <summary>
    /// Provides a general-purpose interface for interacting with a MongoDB database.
    /// </summary>
    /// <typeparam name="T">The type of the document class.</typeparam>
    public class MongoDbHelper<T> : IDatabaseHelper
    {
        private readonly IMongoCollection<T> _collection;
        private readonly IMongoClient _mongoClient;
        private readonly IMongoDatabase _database;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDbHelper{T}"/> class.
        /// </summary>
        /// <param name="connectionString">The MongoDB connection string.</param>
        /// <param name="databaseName">The name of the database.</param>
        /// <param name="collectionName">The name of the collection.</param>
        public MongoDbHelper (string connectionString, string databaseName, string collectionName)
        {
            try
            {
                _mongoClient = new MongoClient(connectionString);
                _database = _mongoClient.GetDatabase(databaseName);
                _collection = _database.GetCollection<T>(collectionName);
                LoggerProvider.Log($"Connected to MongoDB: {databaseName}.{collectionName}", LogLevel.Information);
            }
            catch (Exception ex)
            {
                LoggerProvider.Log($"An error occurred while connecting to MongoDB: {ex.Message}", LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        /// Asynchronously creates a new user in the MongoDB database.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The plain-text password for the user.</param>
        /// <returns>True if the user was successfully created, otherwise false.</returns>
        public async Task<bool> CreateUserAsync (string email, string username, string password)
        {
            try
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                var doc = new BsonDocument
                {
                    { "Email", email },
                    { "Username", username },
                    { "Password", hashedPassword }
                };

                var collection = _database.GetCollection<BsonDocument>(_collection.CollectionNamespace.CollectionName);
                await collection.InsertOneAsync(doc);
                LoggerProvider.Log($"User created: {username}", LogLevel.Information);
                return true;
            }
            catch (Exception ex)
            {
                LoggerProvider.Log($"An error occurred while creating the user: {ex.Message}", LogLevel.Error);
                return false;
            }
        }

        /// <summary>
        /// Deletes a document from the collection.
        /// <code>
        /// // Example:
        /// await mongoDbManager.DeleteAsync(new ObjectId("some_object_id_here"));
        /// </code>
        /// </summary>
        /// <param name="id">The ObjectId of the document to delete.</param>
        public async Task DeleteAsync (ObjectId id)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq("_id", id);
                await _collection.DeleteOneAsync(filter);
                LoggerProvider.Log($"Document deleted: {id}", LogLevel.Information);
            }
            catch (Exception ex)
            {
                LoggerProvider.Log($"An error occurred while deleting the document: {ex.Message}", LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        /// Retrieves all documents from the collection.
        /// <code>
        /// // Example:
        /// var allDocuments = await mongoDbManager.GetAllAsync();
        /// </code>
        /// </summary>
        /// <returns>A list of all documents.</returns>
        public async Task<IEnumerable<T>> GetAllAsync ()
        {
            try
            {
                var documents = await _collection.Find(new BsonDocument()).ToListAsync();
                LoggerProvider.Log($"Retrieved all documents from the collection.", LogLevel.Information);
                return documents;
            }
            catch (Exception ex)
            {
                LoggerProvider.Log($"An error occurred while retrieving all documents: {ex.Message}", LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        /// Retrieves a document by its ObjectId.
        /// <code>
        /// // Example:
        /// var document = await mongoDbManager.GetByIdAsync(new ObjectId("some_object_id_here"));
        /// </code>
        /// </summary>
        /// <param name="id">The ObjectId of the document.</param>
        /// <returns>The document, if found; otherwise, null.</returns>
        public async Task<T> GetByIdAsync (ObjectId id)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq("_id", id);
                var document = await _collection.Find(filter).FirstOrDefaultAsync();
                LoggerProvider.Log($"Retrieved document by ID: {id}", LogLevel.Information);
                return document;
            }
            catch (Exception ex)
            {
                LoggerProvider.Log($"An error occurred while retrieving the document: {ex.Message}", LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        /// Inserts a document into the collection.
        /// <code>
        /// // Example:
        /// await mongoDbManager.InsertAsync(new MyClass { Name = "John", Age = 30 });
        /// </code>
        /// </summary>
        /// <param name="document">The document to insert.</param>
        public async Task InsertAsync (T document)
        {
            try
            {
                await _collection.InsertOneAsync(document);
                LoggerProvider.Log($"Document inserted: {document}", LogLevel.Information);
            }
            catch (Exception ex)
            {
                LoggerProvider.Log($"An error occurred while inserting the document: {ex.Message}", LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        /// Updates a document in the collection.
        /// <code>
        /// // Example:
        /// myClassInstance.Name = "Jane";
        /// await mongoDbManager.UpdateAsync(new ObjectId("some_object_id_here"), myClassInstance);
        /// </code>
        /// </summary>
        /// <param name="id">The ObjectId of the document to update.</param>
        /// <param name="document">The updated document.</param>
        public async Task UpdateAsync (ObjectId id, T document)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq("_id", id);
                await _collection.ReplaceOneAsync(filter, document);
                LoggerProvider.Log($"Document updated: {id}", LogLevel.Information);
            }
            catch (Exception ex)
            {
                LoggerProvider.Log($"An error occurred while updating the document: {ex.Message}", LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        /// Asynchronously verifies if a user with the given email and password exists in the MongoDB database.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The plain-text password of the user.</param>
        /// <returns>True if the user exists and the password is correct, otherwise false.</returns>
        public async Task<bool> VerifyUserAsync (string email, string password)
        {
            try
            {
                var collection = _database.GetCollection<BsonDocument>(_collection.CollectionNamespace.CollectionName);
                var filter = Builders<BsonDocument>.Filter.Eq("Email", email);
                var user = await collection.Find(filter).FirstOrDefaultAsync();

                if (user != null && user.Contains("Password"))
                {
                    bool isVerified = BCrypt.Net.BCrypt.Verify(password, user ["Password"].AsString);
                    LoggerProvider.Log($"User verification result for {email}: {isVerified}", LogLevel.Information);
                    return isVerified;
                }
                return false;
            }
            catch (Exception ex)
            {
                LoggerProvider.Log($"An error occurred while verifying the user: {ex.Message}", LogLevel.Error);
                return false;
            }
        }
    }
}
