using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;

namespace CodingDad.NET.Common.Locators
{
	/// <summary>
	/// Provides a static locator service for resolving dependencies via MEF (Managed Extensibility Framework).
	/// </summary>
	public static class ContainerLocator
	{
		private static readonly object LockObject = new object();
		private static CompositionContainer _container;

		/// <summary>
		/// Composes the parts of a particular object.
		/// </summary>
		/// <param name="obj">The object to compose.</param>
		public static void ComposeParts (object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException(nameof(obj));
			}

			if (_container == null)
			{
				throw new InvalidOperationException("Container must be initialized before use.");
			}

			lock (LockObject)
			{
				_container.ComposeParts(obj);
			}
		}

		/// <summary>
		/// Retrieves the exported value of the specified type T.
		/// </summary>
		/// <typeparam name="T">The type of the exported value.</typeparam>
		/// <param name="contractName">The contract name of the exported value.</param>
		/// <returns>The exported value.</returns>
		public static T GetExportedValue<T> (string contractName = "")
		{
			if (_container == null)
			{
				throw new InvalidOperationException("Container must be initialized before use.");
			}

			return _container.GetExportedValue<T>(contractName);
		}

		/// <summary>
		/// Retrieves the exported values of the specified type T.
		/// </summary>
		/// <typeparam name="T">The type of the exported values.</typeparam>
		/// <param name="contractName">The contract name of the exported values.</param>
		/// <returns>An IEnumerable of exported values.</returns>
		public static IEnumerable<T> GetExportedValues<T> (string contractName = "")
		{
			if (_container == null)
			{
				throw new InvalidOperationException("Container must be initialized before use.");
			}

			return _container.GetExportedValues<T>(contractName) ?? Enumerable.Empty<T>();
		}

		/// <summary>
		/// Initializes the composition container.
		/// </summary>
		/// <param name="container">The MEF composition container.</param>
		public static void Initialize (CompositionContainer container)
		{
			_container = container ?? throw new ArgumentNullException(nameof(container));
		}
	}
}