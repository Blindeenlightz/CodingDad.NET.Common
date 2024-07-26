using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace CodingDad.Common.UserCreationLogin.Utilitities
{
	/// <summary>
	/// Provides functionality to uniquely identify a user based on certain system attributes.
	/// </summary>
	public class UserIdentifier
	{
		private readonly NetworkInterface [] _nics;

		/// <summary>
		/// Initializes a new instance of the <see cref="UserIdentifier"/> class.
		/// </summary>
		/// <param name="nics">An array of NetworkInterface objects.</param>
		public UserIdentifier (NetworkInterface [] nics)
		{
			_nics = nics ?? throw new ArgumentNullException(nameof(nics));
		}

		/// <summary>
		/// Generates a unique identifier for the user based on system attributes.
		/// </summary>
		/// <returns>A unique identifier as a string.</returns>
		/// <exception cref="InvalidOperationException">Thrown when unable to generate a unique identifier.</exception>
		public string GetUniqueIdentifier ()
		{
			try
			{
				if (_nics.Length == 0)
				{
					throw new InvalidOperationException("No network interfaces found.");
				}

				// Combine information from various sources to make it unique per user per machine
				string combinedInfo = $"{Environment.UserName}-{Environment.MachineName}-{_nics [0].GetPhysicalAddress().ToString()}";

				using (SHA256 sha256Hash = SHA256.Create())
				{
					// Compute hash of the combined information
					byte [] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(combinedInfo));

					// Convert byte array to a string
					var sBuilder = new StringBuilder();
					for (int i = 0; i < data.Length; i++)
					{
						sBuilder.Append(data [i].ToString("x2"));
					}

					return sBuilder.ToString();
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Failed to generate unique identifier", ex);
				throw new InvalidOperationException("Unable to generate a unique identifier.", ex);
			}
		}
	}
}