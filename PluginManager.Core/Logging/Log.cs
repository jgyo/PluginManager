namespace PluginManager.Core.Logging
{
    using global::System;
    using global::System.Diagnostics;

    /// <summary>
    /// Defines the <see cref="Log" />.
    /// </summary>
    public class Log : ILog
    {
        /// <summary>
        /// Defines the logLevel.
        /// </summary>
        private readonly LogLevel logLevel;

        /// <summary>
        /// Defines the name.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="logLevel">The logLevel<see cref="LogLevel"/>.</param>
        public Log(string name, LogLevel logLevel)
        {
            this.name = name;
            this.logLevel = logLevel;
        }

        /// <summary>
        /// The IsLogLevelEnabled.
        /// </summary>
        /// <param name="logLevel">The logLevel<see cref="LogLevel"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool IsLogLevelEnabled(LogLevel logLevel)
        {
            return logLevel >= this.logLevel;
        }

        /// <summary>
        /// The Log.
        /// </summary>
        /// <param name="logLevel">The logLevel<see cref="LogLevel"/>.</param>
        /// <param name="messageFunc">The messageFunc<see cref="Func{string}"/>.</param>
        /// <param name="exception">The exception<see cref="Exception"/>.</param>
        /// <param name="formatParameters">The formatParameters<see cref="object[]"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool ILog.Log(LogLevel logLevel, Func<string> messageFunc, Exception exception, params object[] formatParameters)
        {
            if (!IsLogLevelEnabled(logLevel))
                return false;

            if (exception == null)
            {
                Debug.WriteLine($"{logLevel} ({name}): {messageFunc()}");
                return true;
            }

            Debug.WriteLine($"{logLevel} ({name}): {messageFunc()} {Environment.NewLine}{exception.Message}");
            return true;
        }
    }
}
