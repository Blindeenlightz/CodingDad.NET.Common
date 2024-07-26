using CodingDad.Common.Commands;
using CodingDad.Common.DbHelpers;
using CodingDad.Common.UserCreationLogin.Models;
using CodingDad.Common.ViewModels;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace CodingDad.Common.UserCreationLogin.ViewModels
{
	/// <summary>
	/// ViewModel for handling user creation and login functionalities.
	/// </summary>
	public class UserViewModel : BaseViewModel
	{
		private static int _uniqueIdCounter = 100000000;
		private readonly IDatabaseHelper _databaseManager;
		private bool _userLoggedIn;
		private UserModel _userModel;

		/// <summary>
		/// Initializes a new instance of the <see cref="UserViewModel"/> class.
		/// </summary>
		/// <param name="databaseManager">The database manager instance.</param>
		/// <param name="email">The initial email.</param>
		/// <param name="username">The initial username.</param>
		public UserViewModel (IDatabaseHelper databaseManager, string email, string username)
		{
			_databaseManager = databaseManager;
			InitializeModel(email, username);
			LoginCommand = new RelayCommand(param => ValidateUserLogin(((PasswordBox)param).Password));
			GoToCreateUserCommand = new RelayCommand(_ => GoToCreateUser());
			CreateUserCommand = new RelayCommand(param => CreateUser(((PasswordBox)param).Password));
		}

		/// <summary>
		/// Gets the create user command.
		/// </summary>
		public ICommand CreateUserCommand { get; }

		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		public string Email
		{
			get => _userModel.Email;
			set
			{
				if (_userModel.Email != value)
				{
					_userModel.Email = value;
					OnPropertyChanged();
				}
			}
		}

		/// <summary>
		/// Gets the go to create user command.
		/// </summary>
		public ICommand GoToCreateUserCommand { get; }

		/// <summary>
		/// Gets the login command.
		/// </summary>
		public ICommand LoginCommand { get; }

		/// <summary>
		/// Gets or sets the username.
		/// </summary>
		public string Username
		{
			get => _userModel.Username;
			set
			{
				if (_userModel.Username != value)
				{
					_userModel.Username = value;
					OnPropertyChanged();
				}
			}
		}

		/// <summary>
		/// Generates a unique ID.
		/// </summary>
		/// <returns>The generated unique ID.</returns>
		public int GenerateUniqueId ()
		{
			if (_uniqueIdCounter >= 999999999)
			{
				throw new OverflowException("Reached the maximum number of unique IDs");
			}

			return ++_uniqueIdCounter;
		}

		/// <summary>
		/// Checks if the user is logged in.
		/// </summary>
		/// <returns>True if logged in, otherwise false.</returns>
		public bool IsUserLoggedIn () => _userLoggedIn;

		/// <summary>
		/// Creates a new user.
		/// </summary>
		/// <param name="password">The password.</param>
		private async void CreateUser (string password)
		{
			try
			{
				bool isUserCreated = await _databaseManager.CreateUserAsync(_userModel.Email, _userModel.Username, password);

				if (isUserCreated)
				{
					ValidateUserLogin(password);
				}
				else
				{
					// TODO: Log that user was created but login failed
				}
			}
			catch
			{
				// TODO: Log that user creation failed
			}
		}

		/// <summary>
		/// Navigates to the create user view.
		/// </summary>
		private void GoToCreateUser ()
		{
			// TODO: Add logic to switch to CreateUserView
		}

		/// <summary>
		/// Initializes the user model.
		/// </summary>
		/// <param name="email">The email.</param>
		/// <param name="username">The username.</param>
		private void InitializeModel (string email, string username)
		{
			_userModel = new UserModel { Id = GenerateUniqueId() };
			Email = email;
			Username = username;
		}

		/// <summary>
		/// Validates the user login.
		/// </summary>
		/// <param name="password">The password.</param>
		private async void ValidateUserLogin (string password)
		{
			try
			{
				_userLoggedIn = await _databaseManager.VerifyUserAsync(Email, password);
				if (_userLoggedIn)
				{
					// TODO: log successful login
				}
				else
				{
					// TODO: log failed login
				}
			}
			catch
			{
				// TODO: log that something went wrong during login
			}
		}
	}
}