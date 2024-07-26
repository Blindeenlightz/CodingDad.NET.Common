using System;
using System.Windows.Input;

namespace CodingDad.Common.Commands
{
    /// <summary>
    /// A command whose sole purpose is to relay its functionality 
    /// to other objects by invoking delegates. The default return value for the CanExecute method is 'true'.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Predicate<object?>? _canExecute;
        private readonly Action<object> _execute;

        /// <summary>
        /// Initializes a new instance of <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command. This cannot be null.</param>
        /// <remarks><seealso cref="CanExecute"/> will always return true if no canExecute delegate is provided.</remarks>
        public RelayCommand (Action<object> execute)
            : this(null, execute)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="canExecute">Delegate to call on <see cref="CanExecute"/>. This can be null.</param>
        /// <param name="execute">Delegate to execute when <see cref="Execute"/> is called on the command. This cannot be null.</param>
        public RelayCommand (Predicate<object?>? canExecute, Action<object> execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute (object? parameter) => _canExecute is null || _canExecute(parameter);

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute (object parameter) => _execute(parameter);
    }
}
