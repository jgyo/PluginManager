namespace PluginManager.Core.Logging
{
    using global::System;

    /// <summary>
    /// Defines the <see cref="ILog" />.
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// The IsLogLevelEnabled.
        /// </summary>
        /// <param name="logLevel">The logLevel<see cref="LogLevel"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool IsLogLevelEnabled(LogLevel logLevel);

        /// <summary>
        /// The Log.
        /// </summary>
        /// <param name="logLevel">The logLevel<see cref="LogLevel"/>.</param>
        /// <param name="messageFunc">The messageFunc<see cref="Func{string}"/>.</param>
        /// <param name="exception">The exception<see cref="Exception"/>.</param>
        /// <param name="formatParameters">The formatParameters<see cref="object[]"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Log(LogLevel logLevel, Func<string> messageFunc, Exception exception = null, params object[] formatParameters);
    }
}
