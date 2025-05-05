using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CodingDad.NET.Common.ViewModels
{
	public abstract class BaseViewModel : INotifyPropertyChanged, IDisposable
	{
		// Other managed resource this class uses.
		private readonly Component component = new();

		private bool _isDirty;

		// Pointer to an external unmanaged resource.
		private nint handle;

		// Track whether Dispose has been called.
		private bool isDisposed = false;

		// Use C# finalizer syntax for finalization code.
		// This finalizer will run only if the Dispose method
		// does not get called.
		// It gives your base class the opportunity to finalize.
		// Do not provide finalizer in types derived from this class.
		~BaseViewModel ()
		{
			// Do not re-create Dispose clean-up code here.
			// Calling Dispose(disposing: false) is optimal in terms of
			// readability and maintainability.
			Dispose(disposing: false);
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public Guid Id { get; } = new();

		public bool IsDirty
		{
			get { return _isDirty; }
			protected set
			{
				if (_isDirty != value)
				{
					_isDirty = value;
					OnPropertyChanged(nameof(IsDirty));
				}
			}
		}

		public void Dispose ()
		{
			Dispose(disposing: true);
			// This object will be cleaned up by the Dispose method.
			// Therefore, you should call GC.SuppressFinalize to
			// take this object off the finalization queue
			// and prevent finalization code for this object
			// from executing a second time.
			GC.SuppressFinalize(this);
		}

		// Dispose(bool disposing) executes in two distinct scenarios.
		// If disposing equals true, the method has been called directly
		// or indirectly by a user's code. Managed and unmanaged resources
		// can be disposed.
		// If disposing equals false, the method has been called by the
		// runtime from inside the finalizer and you should not reference
		// other objects. Only unmanaged resources can be disposed.
		public virtual void Dispose (bool disposing)
		{
			// Check to see if Dispose has already been called.
			if (!isDisposed)
			{
				// If disposing equals true, dispose all managed
				// and unmanaged resources.
				if (disposing)
				{
					// Dispose managed resources.
					component.Dispose();
				}

				// Call the appropriate methods to clean up
				// unmanaged resources here.
				// If disposing is false,
				// only the following code is executed.
				CloseHandle(handle);
				handle = nint.Zero;

				// Note disposing has been done.
				isDisposed = true;
			}
		}

		public virtual void OnPropertyChanged ([CallerMemberName] string? propertyName = null)
		{
            VerifyPropertyName(propertyName);
			PropertyChangedEventHandler? handler = PropertyChanged;
			if (handler != null)
			{
				var e = new PropertyChangedEventArgs(propertyName);
				handler(this, e);
			}
		}

		/// <summary>
		/// Sets a property and notifies listeners only when the new value is different from the old value.
		/// </summary>
		/// <typeparam name="T">The type of the property.</typeparam>
		/// <param name="storage">Reference to the property's backing field.</param>
		/// <param name="value">The new value to set.</param>
		/// <param name="propertyName">The name of the property. This is auto-filled by the compiler.</param>
		/// <returns>True if the value was changed, false otherwise.</returns>
		protected bool SetProperty<T> (ref T storage, T value, [CallerMemberName] string? propertyName = null)
		{
			if (Equals(storage, value)) return false;

			storage = value;
			OnPropertyChanged(propertyName);
			IsDirty = true;
			return true;
		}

		// Use interop to call the method necessary
		// to clean up the unmanaged resource.
		[System.Runtime.InteropServices.DllImport("Kernel32")]
		private static extern bool CloseHandle (nint handle);

		[Conditional("DEBUG")]
		private void VerifyPropertyName (string? propertyName)
		{
			if (TypeDescriptor.GetProperties(this) [propertyName] == null)
			{
				string msg = "Invalid property name: " + propertyName;
				Debug.Fail(msg);
			}
		}
	}
}