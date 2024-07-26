using System;
using System.IO;

namespace CodingDad.Common.Storage
{
	/// <summary>
	/// A utility class for managing storage locations on a Windows machine.
	/// </summary>
	public static class StorageLocationManager
	{
		/// <summary>
		/// Creates a directory for the application in the AppData folder.
		/// </summary>
		/// <param name="appName">The name of the application.</param>
		/// <returns>The full path of the created directory.</returns>
		/// <exception cref="IOException">Thrown when directory creation fails.</exception>
		/// <remarks>
		/// Recommended for storing user-specific configuration files, databases, and other application-generated data.
		/// Data in this location is tied to the user profile and may not be accessible to other user accounts.
		/// </remarks>
		public static string CreateAppDataDirectory (string appName)
		{
			return CreateDirectory(Environment.SpecialFolder.ApplicationData, appName);
		}

		/// <summary>
		/// Creates a directory for the application in the CommonApplicationData folder.
		/// </summary>
		/// <param name="appName">The name of the application.</param>
		/// <returns>The full path of the created directory.</returns>
		/// <exception cref="IOException">Thrown when directory creation fails.</exception>
		/// <remarks>
		/// Suitable for data that is shared among all users on the system.
		/// May require elevated permissions for write access.
		/// </remarks>
		public static string CreateCommonAppDataDirectory (string appName)
		{
			return CreateDirectory(Environment.SpecialFolder.CommonApplicationData, appName);
		}

		/// <summary>
		/// Creates a directory for the application in a custom folder.
		/// </summary>
		/// <param name="customPath">The custom folder path.</param>
		/// <param name="appName">The name of the application.</param>
		/// <returns>The full path of the created directory.</returns>
		/// <exception cref="IOException">Thrown when directory creation fails.</exception>
		/// <remarks>
		/// Use this method for specialized storage requirements.
		/// Ensure that the application has the necessary permissions to read and write to this location.
		/// </remarks>
		public static string CreateCustomDirectory (string customPath, string appName)
		{
			string fullPath = Path.Combine(customPath, appName);
			CreateDirectoryIfNotExists(fullPath);
			return fullPath;
		}

		/// <summary>
		/// Creates a directory for the application in the LocalApplicationData folder.
		/// </summary>
		/// <param name="appName">The name of the application.</param>
		/// <returns>The full path of the created directory.</returns>
		/// <exception cref="IOException">Thrown when directory creation fails.</exception>
		/// <remarks>
		/// Useful for storing data that is not user-specific but should not be shared with other users.
		/// Data stored here is generally tied to a single machine.
		/// </remarks>
		public static string CreateLocalAppDataDirectory (string appName)
		{
			return CreateDirectory(Environment.SpecialFolder.LocalApplicationData, appName);
		}

		/// <summary>
		/// Creates a directory for the application in the system's temporary folder.
		/// </summary>
		/// <param name="appName">The name of the application.</param>
		/// <returns>The full path of the created directory.</returns>
		/// <exception cref="IOException">Thrown when directory creation fails.</exception>
		/// <remarks>
		/// Suitable for temporary files that don't require persistence across application restarts.
		/// Contents may be deleted at any time.
		/// </remarks>
		public static string CreateTempDirectory (string appName)
		{
			string tempPath = Path.GetTempPath();
			string appSpecificTempFolder = Path.Combine(tempPath, appName);
			CreateDirectoryIfNotExists(appSpecificTempFolder);
			return appSpecificTempFolder;
		}

		private static string CreateDirectory (Environment.SpecialFolder folder, string appName)
		{
			string folderPath = Environment.GetFolderPath(folder);
			string appSpecificFolder = Path.Combine(folderPath, appName);
			CreateDirectoryIfNotExists(appSpecificFolder);
			return appSpecificFolder;
		}

		private static void CreateDirectoryIfNotExists (string path)
		{
			try
			{
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
			}
			catch (Exception ex)
			{
				throw new IOException($"Failed to create directory at {path}.", ex);
			}
		}
	}
}