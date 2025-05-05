using CodingDad.NET.Common.Constants;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CodingDad.NET.Common.Loggers.ColorConsoleLogger
{
	/// <summary>
	/// Provides instances of <see cref="ColorConsoleLogger"/> with dynamic configuration support.
	/// </summary>
	[ProviderAlias("ColorConsole")]
	[Export(MefConstants.ColorConsoleLoggerProvider.ContractName, typeof(ILoggerProvider))]
	public sealed class ColorConsoleLoggerProvider : ILoggerProvider
	{
		/// <summary>
		/// Thread-safe dictionary to hold and cache logger instances.
		/// </summary>
		private readonly ConcurrentDictionary<string, ColorConsoleLogger> _loggers = new(StringComparer.OrdinalIgnoreCase);

		private readonly IDisposable? _onChangeToken;
		private ColorConsoleLoggerConfiguration _currentConfig;

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorConsoleLoggerProvider"/> class,
		/// loading its configuration from an embedded JSON file.
		/// </summary>
		public ColorConsoleLoggerProvider ()
		{
			_currentConfig = LoadEmbeddedConfig();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorConsoleLoggerProvider"/> class
		/// using dynamic configuration options.
		/// </summary>
		/// <param name="config">The dynamic configuration options for the logger.</param>
		/// <exception cref="ArgumentNullException">Thrown when the config parameter is null.</exception>
		public ColorConsoleLoggerProvider (IOptionsMonitor<ColorConsoleLoggerConfiguration> config)
		{
			_currentConfig = config?.CurrentValue ?? throw new ArgumentNullException(nameof(config));
			_onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig ?? _currentConfig);
		}

		/// <summary>
		/// Creates or retrieves a logger instance with the specified category name.
		/// </summary>
		/// <param name="categoryName">The category name for messages produced by the logger.</param>
		/// <param name="outputTarget">The output target for the logger. Optional, default is the debug output window.</param>
		/// <returns>An instance of <see cref="ColorConsoleLogger"/>.</returns>
		public ILogger CreateLogger (string categoryName, LoggerOutputTarget outputTarget) => _loggers.GetOrAdd(categoryName, name => new ColorConsoleLogger(name, GetCurrentConfig, outputTarget));

		/// <summary>
		/// Creates or retrieves a logger instance with the specified category name.
		/// </summary>
		/// <param name="categoryName">The category name for messages produced by the logger.</param>
		/// <returns>An instance of <see cref="ColorConsoleLogger"/>.</returns>
		public ILogger CreateLogger (string categoryName) => _loggers.GetOrAdd(categoryName, name => new ColorConsoleLogger(name, GetCurrentConfig, LoggerOutputTarget.DebugWindow));

		/// <summary>
		/// Disposes of resources used by this instance.
		/// </summary>
		public void Dispose ()
		{
			_loggers.Clear();
			_onChangeToken?.Dispose();
		}

		/// <summary>
		/// Retrieves the current configuration for the logger.
		/// </summary>
		/// <returns>The current <see cref="ColorConsoleLoggerConfiguration"/>.</returns>
		private ColorConsoleLoggerConfiguration GetCurrentConfig () => _currentConfig;

        /// <summary>
        /// Loads the logger configuration from an embedded JSON file.
        /// </summary>
        /// <returns>The populated <see cref="ColorConsoleLoggerConfiguration"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the embedded resource is not found.</exception>
        private ColorConsoleLoggerConfiguration LoadEmbeddedConfig ()
        {
            try
            {
                var asm = Assembly.GetExecutingAssembly();

                // Find the resource whose name ends with our JSON filename
                var resourceName = asm.GetManifestResourceNames()
                                      .FirstOrDefault(n => n.EndsWith("colorloggersettings.json", StringComparison.OrdinalIgnoreCase));

                if (resourceName is null)
                    throw new InvalidOperationException("Embedded colorloggersettings.json not found.");

                using var stream = asm.GetManifestResourceStream(resourceName)
                                 ?? throw new InvalidOperationException($"Could not load embedded resource '{resourceName}'.");

                using var reader = new StreamReader(stream);
                var configJson = reader.ReadToEnd();

                return JsonConvert.DeserializeObject<ColorConsoleLoggerConfiguration>(configJson)
                       ?? throw new InvalidOperationException("Deserialization of colorloggersettings.json returned null.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw new InvalidOperationException("Failed to load embedded configuration.", ex);
            }
        }
    }
}