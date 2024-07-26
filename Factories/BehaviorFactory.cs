using Microsoft.Xaml.Behaviors;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace CodingDad.Common.Factories
{
	/// <summary>
	/// Factory for creating and injecting dependencies into Behavior instances.
	/// </summary>
	public static class BehaviorFactory
	{
		private static readonly object _lock = new object();
		private static CompositionContainer _compositionContainer;

		/// <summary>
		/// Creates an instance of a Behavior with constructor parameters.
		/// </summary>
		/// <typeparam name="T">The type of Behavior to create.</typeparam>
		/// <param name="constructorArgs">The arguments to pass to the constructor.</param>
		/// <returns>An instance of the Behavior.</returns>
		public static T CreateBehavior<T> (params object [] constructorArgs) where T : Behavior
		{
			EnsureInitialized();
			var behavior = (T)Activator.CreateInstance(typeof(T), constructorArgs);
			ValidateInstance(behavior, typeof(T));
			SatisfyImports(behavior);
			return behavior;
		}

		/// <summary>
		/// General-purpose method to create an instance of any Behavior type.
		/// </summary>
		/// <param name="type">The type of Behavior to create.</param>
		/// <returns>An instance of the Behavior.</returns>
		public static Behavior CreateBehavior (Type type)
		{
			EnsureInitialized();
			var instance = (Behavior)Activator.CreateInstance(type);
			ValidateInstance(instance, type);
			SatisfyImports(instance);
			return instance;
		}

		/// <summary>
		/// Initializes the BehaviorFactory with a CompositionContainer for dependency injection.
		/// </summary>
		/// <param name="compositionContainer">The MEF CompositionContainer.</param>
		public static void Initialize (CompositionContainer compositionContainer)
		{
			_compositionContainer = compositionContainer ?? throw new ArgumentNullException(nameof(compositionContainer));
		}

		/// <summary>
		/// Ensures that the factory has been initialized.
		/// </summary>
		private static void EnsureInitialized ()
		{
			if (_compositionContainer == null)
			{
				throw new InvalidOperationException("BehaviorFactory must be initialized before use.");
			}
		}

		/// <summary>
		/// Internal method to perform MEF composition on the instance.
		/// </summary>
		/// <param name="instance">The Behavior instance to inject dependencies into.</param>
		private static void SatisfyImports (Behavior instance)
		{
			lock (_lock)
			{
				_compositionContainer.SatisfyImportsOnce(instance);
			}
		}

		/// <summary>
		/// Internal method to validate that an instance was created successfully.
		/// </summary>
		/// <param name="instance">The created Behavior instance.</param>
		/// <param name="type">The type of the Behavior.</param>
		private static void ValidateInstance (Behavior instance, Type type)
		{
			if (instance == null)
			{
				throw new InvalidOperationException($"Failed to create an instance of {type.FullName}");
			}
		}
	}
}