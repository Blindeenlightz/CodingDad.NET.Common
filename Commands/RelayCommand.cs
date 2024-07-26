using System;
using System.Windows.Input;

namespace CodingDad.Common.Commands
{
	public class RelayCommand : ICommand
	{
		private readonly Predicate<object?>? _canExecute;
		private readonly Action<object> _execute;

		/// <summary>
		/// Initializes a new instance of <see cref="DelegateCommand{T}"/>.
		/// </summary>
		/// <param name="execute">Delegate to execute when Execute is called on the command.  This can be null to just hook up a CanExecute delegate.</param>
		/// <remarks><seealso cref="CanExecute"/> will always return true.</remarks>
		public RelayCommand (Action<object> execute)
			: this(null, execute)
		{
		}

		public RelayCommand (Predicate<object?>? canExecute, Action<object> execute)
		{
			if (execute is null)
			{
				throw new ArgumentNullException("execute");
			}

			_canExecute = canExecute;
			_execute = execute;
		}

		public event EventHandler? CanExecuteChanged
		{
			add => CommandManager.RequerySuggested += value;
			remove => CommandManager.RequerySuggested -= value;
		}

		public bool CanExecute (object? parameter) => _canExecute is null || _canExecute(parameter);

		public void Execute (object parameter) => _execute(parameter);
	}
}