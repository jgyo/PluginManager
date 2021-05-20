namespace PluginManager.Core.Logging
{
    /// <summary>
    /// Defines the <see cref="ILogProvider" />.
    /// </summary>
    public interface ILogProvider
    {
        /// <summary>
        /// Gets or sets a value indicating whether LoggingEnabled.
        /// </summary>
        bool LoggingEnabled { get; set; }

        /// <summary>
        /// Gets or sets the LogLevel.
        /// </summary>
        LogLevel LogLevel { get; set; }

        /// <summary>
        /// The GetLogFor.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <returns>The <see cref="ILog"/>.</returns>
        ILog GetLogFor<T>();

        /// <summary>
        /// The GetLogFor.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="ILog"/>.</returns>
        ILog GetLogFor(string name);

        /// <summary>
        /// The GetLogFor.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="caller">The caller<see cref="T"/>.</param>
        /// <returns>The <see cref="ILog"/>.</returns>
        ILog GetLogFor<T>(T caller);
    }
}
