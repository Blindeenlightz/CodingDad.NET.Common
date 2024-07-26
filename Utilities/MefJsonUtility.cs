using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Text.Json;

namespace CodingDad.Common.Utilities
{
	/// <summary>
	/// <summary>
	/// Utility class for serializing and deserializing objects while also managing MEF imports.
	/// </summary>
	public static class MefJsonUtility
	{
		private static CompositionContainer _container;

		/// <summary>
		/// Deserializes a JSON string to an object and satisfies MEF imports.
		/// </summary>
		/// <typeparam name="T">The type of the object.</typeparam>
		/// <param name="json">The JSON string.</param>
		/// <returns>The deserialized object with MEF imports satisfied.</returns>
		public static T DeserializeAndSatisfyImports<T> (string json) where T : class
		{
			if (_container == null)
			{
				throw new InvalidOperationException("MefJsonUtility has not been initialized.");
			}

			try
			{
				T obj = JsonSerializer.Deserialize<T>(json);
				if (obj != null)
				{
					try
					{
						_container.ComposeParts(obj);
					}
					catch (Exception mefEx)
					{
						// Handle or log MEF-related exception as needed
						throw new InvalidOperationException("Failed to satisfy MEF imports.", mefEx);
					}
				}
				return obj;
			}
			catch (Exception ex)
			{
				// Handle or log exception as needed
				throw new InvalidOperationException("Failed to deserialize object or satisfy imports.", ex);
			}
		}

		/// <summary>
		/// Initializes the MefJsonUtility with a CompositionContainer.
		/// </summary>
		/// <param name="container">The MEF CompositionContainer instance.</param>
		public static void Initialize (CompositionContainer container)
		{
			_container = container ?? throw new ArgumentNullException(nameof(container));
		}

		/// <summary>
		/// Serializes an object to a JSON string.
		/// </summary>
		/// <typeparam name="T">The type of the object.</typeparam>
		/// <param name="obj">The object to serialize.</param>
		/// <returns>The JSON string.</returns>
		public static string Serialize<T> (T obj)
		{
			if (_container == null)
			{
				throw new InvalidOperationException("MefJsonUtility has not been initialized.");
			}

			try
			{
				return JsonSerializer.Serialize(obj);
			}
			catch (Exception ex)
			{
				// Handle or log exception as needed
				throw new InvalidOperationException("Failed to serialize object.", ex);
			}
		}
	}
}