using CodingDad.Common.Loggers;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace CodingDad.Common.Factories
{
	/// <summary>
	/// A static factory class for creating ViewModel instances and satisfying their dependencies via MEF.
	/// </summary>
	public static class ViewModelFactory
	{
		private static readonly object _lock = new object();
		private static CompositionContainer _compositionContainer;

		/// <summary>
		/// Creates an instance of a ViewModel of type T, satisfying its dependencies using MEF.
		/// </summary>
		/// <typeparam name="T">The type of ViewModel to create.</typeparam>
		/// <returns>An instance of the ViewModel with its dependencies satisfied.</returns>
		/// <remarks>This method is appropriate for ViewModel classes that have a parameterless constructor.</remarks>
		public static T CreateViewModel<T> () where T : new()
		{
			EnsureInitialized();
			var viewModel = new T();
			SatisfyImports(viewModel);
			return viewModel;
		}

		/// <summary>
		/// Creates an instance of a ViewModel of type T, satisfying its dependencies using MEF.
		/// </summary>
		/// <typeparam name="T">The type of ViewModel to create.</typeparam>
		/// <param name="constructorArgs">Arguments for the ViewModel's constructor.</param>
		/// <returns>An instance of the ViewModel with its dependencies satisfied.</returns>
		/// <remarks>This method is useful for ViewModel classes that require constructor arguments.</remarks>
		public static T CreateViewModel<T> (params object [] constructorArgs) where T : class
		{
			var viewModel = default(T);
			try
			{
				EnsureInitialized();
				LoggerProvider.Log($"Creating ViewModel of type {typeof(T).FullName}", Microsoft.Extensions.Logging.LogLevel.Debug);
				foreach(var arg in constructorArgs)
				{
					LoggerProvider.Log($"Constructor arg: {arg}", Microsoft.Extensions.Logging.LogLevel.Debug);
				}
				viewModel = (T)Activator.CreateInstance(typeof(T), constructorArgs);
				ValidateInstance(viewModel, typeof(T));
				SatisfyImports(viewModel);
			}
			catch (Exception ex)
			{
				LoggerProvider.Log($"Failed to create ViewModel of type {typeof(T).FullName}: {ex.Message}", Microsoft.Extensions.Logging.LogLevel.Error);
				throw;
			}

			return viewModel;
		}

		/// <summary>
		/// Creates an instance of a specified type, satisfying its dependencies using MEF.
		/// </summary>
		/// <param name="type">The type of object to create.</param>
		/// <returns>An instance of the object with its dependencies satisfied.</returns>
		public static object CreateViewModel (Type type)
		{
			EnsureInitialized();
			var instance = Activator.CreateInstance(type);
			ValidateInstance(instance, type);
			SatisfyImports(instance);
			return instance;
		}

		/// <summary>
		/// Initializes the ViewModelFactory with a MEF CompositionContainer.
		/// </summary>
		/// <param name="compositionContainer">The MEF CompositionContainer.</param>
		public static void Initialize (CompositionContainer compositionContainer)
		{
			_compositionContainer = compositionContainer ?? throw new ArgumentNullException(nameof(compositionContainer));
		}

		/// <summary>
		/// Ensures that the ViewModelFactory is initialized before it is used.
		/// </summary>
		private static void EnsureInitialized ()
		{
			if (_compositionContainer == null)
			{
				throw new InvalidOperationException("ViewModelFactory must be initialized before use.");
			}
		}

		/// <summary>
		/// Uses the MEF CompositionContainer to satisfy the imports of the provided instance.
		/// </summary>
		/// <param name="instance">The instance whose dependencies need to be satisfied.</param>
		private static void SatisfyImports (object instance)
		{
			//lock (_lock)
			//{
				_compositionContainer.SatisfyImportsOnce(instance);
			//}
		}

		/// <summary>
		/// Validates that an instance was successfully created.
		/// </summary>
		/// <param name="instance">The created instance.</param>
		/// <param name="type">The type of the instance.</param>
		private static void ValidateInstance (object instance, Type type)
		{
			if (instance == null)
			{
				throw new InvalidOperationException($"Failed to create an instance of {type.FullName}");
			}
		}
	}
}