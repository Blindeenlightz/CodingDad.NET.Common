using CodingDad.NET.Common.Commands;
using CodingDad.NET.Common.ViewModels;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
using CodingDad.NET.Common.DbHelpers;
using CodingDad.NET.Common.Loggers;
using CodingDad.NET.Common.UserCreationLogin.Models;

namespace CodingDad.NET.Common.UserCreationLogin.ViewModels
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
                LoggerProvider.Log($"Attempting to create user: {_userModel.Username}", LogLevel.Information);
                bool isUserCreated = await _databaseManager.CreateUserAsync(_userModel.Email, _userModel.Username, password);

                if (isUserCreated)
                {
                    LoggerProvider.Log($"User created successfully: {_userModel.Username}", LogLevel.Information);
                    ValidateUserLogin(password);
                }
                else
                {
                    LoggerProvider.Log($"User creation failed: {_userModel.Username}", LogLevel.Warning);
                }
            }
            catch (Exception ex)
            {
                LoggerProvider.Log($"Error creating user: {_userModel.Username}, Exception: {ex.Message}", LogLevel.Error);
            }
        }

        /// <summary>
        /// Navigates to the create user view.
        /// </summary>
        private void GoToCreateUser ()
        {
            LoggerProvider.Log("Navigating to CreateUserView", LogLevel.Information);
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
                LoggerProvider.Log($"Attempting to log in user: {_userModel.Username}", LogLevel.Information);
                _userLoggedIn = await _databaseManager.VerifyUserAsync(Email, password);

                if (_userLoggedIn)
                {
                    LoggerProvider.Log($"User logged in successfully: {_userModel.Username}", LogLevel.Information);
                }
                else
                {
                    LoggerProvider.Log($"User login failed: {_userModel.Username}", LogLevel.Warning);
                }
            }
            catch (Exception ex)
            {
                LoggerProvider.Log($"Error during login: {_userModel.Username}, Exception: {ex.Message}", LogLevel.Error);
            }
        }
    }
}
