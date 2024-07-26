using System.Threading.Tasks;

namespace CodingDad.Common.DbHelpers
{
	public interface IDatabaseHelper
	{
		/// <summary>
		/// Creates a new user in the database asynchronously.
		/// </summary>
		/// <param name="user">The user model.</param>
		/// <returns>True if user is created successfully, false otherwise.</returns>
		Task<bool> CreateUserAsync (string email, string username, string password);

		/// <summary>
		/// Verifies the user credentials asynchronously.
		/// </summary>
		/// <param name="email">Email of the user.</param>
		/// <param name="password">Password of the user.</param>
		/// <returns>True if credentials are valid, false otherwise.</returns>
		Task<bool> VerifyUserAsync (string email, string password);
	}
}
