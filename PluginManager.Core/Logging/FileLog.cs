namespace PluginManager.Core.Logging
{
    using global::System;
    using global::System.IO;

    /// <summary>
    /// Defines the <see cref="FileLog" />.
    /// </summary>
    public class FileLog : ILog
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
        /// Defines the filePath.
        /// </summary>
        private static string filePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLog"/> class.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="logLevel">The logLevel<see cref="LogLevel"/>.</param>
        public FileLog(string name, LogLevel logLevel)
        {
            this.name = name;
            this.logLevel = logLevel;
        }

        /// <summary>
        /// Initializes static members of the <see cref="FileLog"/> class.
        /// </summary>
        static FileLog()
        {
            filePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Yoder\\PluginManager\\pm.log");
        }

        /// <summary>
        /// Gets the FilePath.
        /// </summary>
        public static string FilePath => filePath;

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
                File.AppendAllText(filePath, $"{logLevel} ({name}): {messageFunc()}\n");
                return true;
            }

            File.AppendAllText(filePath, $"{logLevel} ({name}): {messageFunc()} {Environment.NewLine}{exception.Message}\n");
            return true;
        }
    }
}
