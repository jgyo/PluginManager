namespace PluginManager.Core.Logging
{
    using global::System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="FileLogProvider" />.
    /// </summary>
    public class FileLogProvider : ILogProvider
    {
        /// <summary>
        /// Defines the logs.
        /// </summary>
        private readonly Dictionary<string, ILog> logs = new();

        /// <summary>
        /// Defines the loggingEnabled.
        /// </summary>
        private bool loggingEnabled = false;

        /// <summary>
        /// Defines the logLevel.
        /// </summary>
        private LogLevel logLevel = LogLevel.Trace;

        /// <summary>
        /// Gets the Instance.
        /// </summary>
        public static FileLogProvider Instance { get; } = new FileLogProvider();

        /// <summary>
        /// Gets or sets a value indicating whether LoggingEnabled.
        /// </summary>
        public bool LoggingEnabled
        {
            get => loggingEnabled;
            set
            {
                if (loggingEnabled == value)
                    return;
                loggingEnabled = value;
                logs.Clear();
            }
        }

        /// <summary>
        /// Gets or sets the LogLevel.
        /// </summary>
        public LogLevel LogLevel
        {
            get => logLevel;
            set
            {
                if (logLevel == value)
                    return;
                logLevel = value;
                logs.Clear();
            }
        }

        /// <summary>
        /// The GetLogFor.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <returns>The <see cref="ILog"/>.</returns>
        public ILog GetLogFor<T>()
        {
            var name = typeof(T).FullName;
            return GetLogFor(name);
        }

        /// <summary>
        /// The GetLogFor.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="ILog"/>.</returns>
        public ILog GetLogFor(string name)
        {
            if (logs.ContainsKey(name))
                return logs[name];

            ILog log = LoggingEnabled ? new FileLog(name, LogLevel) : (ILog)new DisabledLog();
            logs.Add(name, log);

            return log;
        }

        /// <summary>
        /// The GetLogFor.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="caller">The caller<see cref="T"/>.</param>
        /// <returns>The <see cref="ILog"/>.</returns>
        public ILog GetLogFor<T>(T caller)
        {
            var name = caller.GetType().FullName;
            return GetLogFor(name);
        }
    }
}
