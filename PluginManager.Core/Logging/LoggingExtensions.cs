namespace PluginManager.Core.Logging
{
    using global::System;

    // This class copied from MvvmCross
    /// <summary>
    /// Defines the <see cref="LoggingExtensions" />.
    /// </summary>
    public static class LoggingExtensions
    {
        /// <summary>
        /// The Debug.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="exception">The exception<see cref="Exception"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="args">The args<see cref="object[]"/>.</param>
        public static void Debug(this ILog logger, Exception exception, string message, params object[] args)
        {
            logger.DebugException(message, exception, args);
        }

        /// <summary>
        /// The Debug.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="messageFunc">The messageFunc<see cref="Func{string}"/>.</param>
        public static void Debug(this ILog logger, Func<string> messageFunc)
        {
            GuardAgainstNullLogger(logger);
            logger.Log(LogLevel.Debug, messageFunc);
        }

        /// <summary>
        /// The Debug.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        public static void Debug(this ILog logger, string message)
        {
            if (logger.IsDebugEnabled())
            {
                logger.Log(LogLevel.Debug, message.AsFunc());
            }
        }

        /// <summary>
        /// The Debug.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="args">The args<see cref="object[]"/>.</param>
        public static void Debug(this ILog logger, string message, params object[] args)
        {
            logger.DebugFormat(message, args);
        }

        /// <summary>
        /// The DebugException.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="exception">The exception<see cref="Exception"/>.</param>
        public static void DebugException(this ILog logger, string message, Exception exception)
        {
            if (logger.IsDebugEnabled())
            {
                logger.Log(LogLevel.Debug, message.AsFunc(), exception);
            }
        }

        /// <summary>
        /// The DebugException.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="exception">The exception<see cref="Exception"/>.</param>
        /// <param name="formatParams">The formatParams<see cref="object[]"/>.</param>
        public static void DebugException(this ILog logger, string message, Exception exception, params object[] formatParams)
        {
            if (logger.IsDebugEnabled())
            {
                logger.Log(LogLevel.Debug, message.AsFunc(), exception, formatParams);
            }
        }

        /// <summary>
        /// The DebugFormat.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="args">The args<see cref="object[]"/>.</param>
        public static void DebugFormat(this ILog logger, string message, params object[] args)
        {
            if (logger.IsDebugEnabled())
            {
                logger.LogFormat(LogLevel.Debug, message, args);
            }
        }

        /// <summary>
        /// The Error.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="exception">The exception<see cref="Exception"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="args">The args<see cref="object[]"/>.</param>
        public static void Error(this ILog logger, Exception exception, string message, params object[] args)
        {
            logger.ErrorException(message, exception, args);
        }

        /// <summary>
        /// The Error.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="messageFunc">The messageFunc<see cref="Func{string}"/>.</param>
        public static void Error(this ILog logger, Func<string> messageFunc)
        {
            GuardAgainstNullLogger(logger);
            logger.Log(LogLevel.Error, messageFunc);
        }

        /// <summary>
        /// The Error.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        public static void Error(this ILog logger, string message)
        {
            if (logger.IsErrorEnabled())
            {
                logger.Log(LogLevel.Error, message.AsFunc());
            }
        }

        /// <summary>
        /// The Error.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="args">The args<see cref="object[]"/>.</param>
        public static void Error(this ILog logger, string message, params object[] args)
        {
            logger.ErrorFormat(message, args);
        }

        /// <summary>
        /// The ErrorException.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="exception">The exception<see cref="Exception"/>.</param>
        /// <param name="formatParams">The formatParams<see cref="object[]"/>.</param>
        public static void ErrorException(this ILog logger, string message, Exception exception, params object[] formatParams)
        {
            if (logger.IsErrorEnabled())
            {
                logger.Log(LogLevel.Error, message.AsFunc(), exception, formatParams);
            }
        }

        /// <summary>
        /// The ErrorFormat.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="args">The args<see cref="object[]"/>.</param>
        public static void ErrorFormat(this ILog logger, string message, params object[] args)
        {
            if (logger.IsErrorEnabled())
            {
                logger.LogFormat(LogLevel.Error, message, args);
            }
        }

        /// <summary>
        /// The Fatal.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="exception">The exception<see cref="Exception"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="args">The args<see cref="object[]"/>.</param>
        public static void Fatal(this ILog logger, Exception exception, string message, params object[] args)
        {
            logger.FatalException(message, exception, args);
        }

        /// <summary>
        /// The Fatal.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="messageFunc">The messageFunc<see cref="Func{string}"/>.</param>
        public static void Fatal(this ILog logger, Func<string> messageFunc)
        {
            logger.Log(LogLevel.Fatal, messageFunc);
        }

        /// <summary>
        /// The Fatal.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        public static void Fatal(this ILog logger, string message)
        {
            if (logger.IsFatalEnabled())
            {
                logger.Log(LogLevel.Fatal, message.AsFunc());
            }
        }

        /// <summary>
        /// The Fatal.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="args">The args<see cref="object[]"/>.</param>
        public static void Fatal(this ILog logger, string message, params object[] args)
        {
            logger.FatalFormat(message, args);
        }

        /// <summary>
        /// The FatalException.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="exception">The exception<see cref="Exception"/>.</param>
        /// <param name="formatParams">The formatParams<see cref="object[]"/>.</param>
        public static void FatalException(this ILog logger, string message, Exception exception, params object[] formatParams)
        {
            if (logger.IsFatalEnabled())
            {
                logger.Log(LogLevel.Fatal, message.AsFunc(), exception, formatParams);
            }
        }

        /// <summary>
        /// The FatalFormat.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="args">The args<see cref="object[]"/>.</param>
        public static void FatalFormat(this ILog logger, string message, params object[] args)
        {
            if (logger.IsFatalEnabled())
            {
                logger.LogFormat(LogLevel.Fatal, message, args);
            }
        }

        /// <summary>
        /// The Info.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="exception">The exception<see cref="Exception"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="args">The args<see cref="object[]"/>.</param>
        public static void Info(this ILog logger, Exception exception, string message, params object[] args)
        {
            logger.InfoException(message, exception, args);
        }

        /// <summary>
        /// The Info.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="messageFunc">The messageFunc<see cref="Func{string}"/>.</param>
        public static void Info(this ILog logger, Func<string> messageFunc)
        {
            GuardAgainstNullLogger(logger);
            logger.Log(LogLevel.Info, messageFunc);
        }

        /// <summary>
        /// The Info.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        public static void Info(this ILog logger, string message)
        {
            if (logger.IsInfoEnabled())
            {
                logger.Log(LogLevel.Info, message.AsFunc());
            }
        }

        /// <summary>
        /// The Info.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="args">The args<see cref="object[]"/>.</param>
        public static void Info(this ILog logger, string message, params object[] args)
        {
            logger.InfoFormat(message, args);
        }

        /// <summary>
        /// The InfoException.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="exception">The exception<see cref="Exception"/>.</param>
        /// <param name="formatParams">The formatParams<see cref="object[]"/>.</param>
        public static void InfoException(this ILog logger, string message, Exception exception, params object[] formatParams)
        {
            if (logger.IsInfoEnabled())
            {
                logger.Log(LogLevel.Info, message.AsFunc(), exception, formatParams);
            }
        }

        /// <summary>
        /// The InfoFormat.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="args">The args<see cref="object[]"/>.</param>
        public static void InfoFormat(this ILog logger, string message, params object[] args)
        {
            if (logger.IsInfoEnabled())
            {
                logger.LogFormat(LogLevel.Info, message, args);
            }
        }

        /// <summary>
        /// The IsDebugEnabled.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsDebugEnabled(this ILog logger)
        {
            GuardAgainstNullLogger(logger);
            return logger.IsLogLevelEnabled(LogLevel.Debug);
        }

        /// <summary>
        /// The IsErrorEnabled.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsErrorEnabled(this ILog logger)
        {
            GuardAgainstNullLogger(logger);
            return logger.IsLogLevelEnabled(LogLevel.Error);
        }

        /// <summary>
        /// The IsFatalEnabled.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsFatalEnabled(this ILog logger)
        {
            GuardAgainstNullLogger(logger);
            return logger.IsLogLevelEnabled(LogLevel.Fatal);
        }

        /// <summary>
        /// The IsInfoEnabled.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsInfoEnabled(this ILog logger)
        {
            GuardAgainstNullLogger(logger);
            return logger.IsLogLevelEnabled(LogLevel.Info);
        }

        /// <summary>
        /// The IsTraceEnabled.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsTraceEnabled(this ILog logger)
        {
            GuardAgainstNullLogger(logger);
            return logger.IsLogLevelEnabled(LogLevel.Trace);
        }

        /// <summary>
        /// The IsWarnEnabled.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsWarnEnabled(this ILog logger)
        {
            GuardAgainstNullLogger(logger);
            return logger.IsLogLevelEnabled(LogLevel.Warn);
        }

        /// <summary>
        /// The Trace.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="exception">The exception<see cref="Exception"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="args">The args<see cref="object[]"/>.</param>
        public static void Trace(this ILog logger, Exception exception, string message, params object[] args)
        {
            logger.TraceException(message, exception, args);
        }

        /// <summary>
        /// The Trace.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="messageFunc">The messageFunc<see cref="Func{string}"/>.</param>
        public static void Trace(this ILog logger, Func<string> messageFunc)
        {
            GuardAgainstNullLogger(logger);
            logger.Log(LogLevel.Trace, messageFunc);
        }

        /// <summary>
        /// The Trace.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        public static void Trace(this ILog logger, string message)
        {
            if (logger.IsTraceEnabled())
            {
                logger.Log(LogLevel.Trace, message.AsFunc());
            }
        }

        /// <summary>
        /// The Trace.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="args">The args<see cref="object[]"/>.</param>
        public static void Trace(this ILog logger, string message, params object[] args)
        {
            logger.TraceFormat(message, args);
        }

        /// <summary>
        /// The TraceException.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="exception">The exception<see cref="Exception"/>.</param>
        /// <param name="formatParams">The formatParams<see cref="object[]"/>.</param>
        public static void TraceException(this ILog logger, string message, Exception exception, params object[] formatParams)
        {
            if (logger.IsTraceEnabled())
            {
                logger.Log(LogLevel.Trace, message.AsFunc(), exception, formatParams);
            }
        }

        /// <summary>
        /// The TraceFormat.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="args">The args<see cref="object[]"/>.</param>
        public static void TraceFormat(this ILog logger, string message, params object[] args)
        {
            if (logger.IsTraceEnabled())
            {
                logger.LogFormat(LogLevel.Trace, message, args);
            }
        }

        /// <summary>
        /// The Warn.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="exception">The exception<see cref="Exception"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="args">The args<see cref="object[]"/>.</param>
        public static void Warn(this ILog logger, Exception exception, string message, params object[] args)
        {
            logger.WarnException(message, exception, args);
        }

        /// <summary>
        /// The Warn.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="messageFunc">The messageFunc<see cref="Func{string}"/>.</param>
        public static void Warn(this ILog logger, Func<string> messageFunc)
        {
            GuardAgainstNullLogger(logger);
            logger.Log(LogLevel.Warn, messageFunc);
        }

        /// <summary>
        /// The Warn.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        public static void Warn(this ILog logger, string message)
        {
            if (logger.IsWarnEnabled())
            {
                logger.Log(LogLevel.Warn, message.AsFunc());
            }
        }

        /// <summary>
        /// The Warn.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="args">The args<see cref="object[]"/>.</param>
        public static void Warn(this ILog logger, string message, params object[] args)
        {
            logger.WarnFormat(message, args);
        }

        /// <summary>
        /// The WarnException.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="exception">The exception<see cref="Exception"/>.</param>
        /// <param name="formatParams">The formatParams<see cref="object[]"/>.</param>
        public static void WarnException(this ILog logger, string message, Exception exception, params object[] formatParams)
        {
            if (logger.IsWarnEnabled())
            {
                logger.Log(LogLevel.Warn, message.AsFunc(), exception, formatParams);
            }
        }

        /// <summary>
        /// The WarnFormat.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="args">The args<see cref="object[]"/>.</param>
        public static void WarnFormat(this ILog logger, string message, params object[] args)
        {
            if (logger.IsWarnEnabled())
            {
                logger.LogFormat(LogLevel.Warn, message, args);
            }
        }

        // Avoid the closure allocation, see https://gist.github.com/AArnott/d285feef75c18f6ecd2b
        /// <summary>
        /// The AsFunc.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="value">The value<see cref="T"/>.</param>
        /// <returns>The <see cref="Func{T}"/>.</returns>
        private static Func<T> AsFunc<T>(this T value) where T : class
        {
            return value.Return;
        }

        // ReSharper disable once UnusedParameter.Local
        /// <summary>
        /// The GuardAgainstNullLogger.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        private static void GuardAgainstNullLogger(ILog logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }
        }

        /// <summary>
        /// The LogFormat.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILog"/>.</param>
        /// <param name="logLevel">The logLevel<see cref="LogLevel"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="args">The args<see cref="object[]"/>.</param>
        private static void LogFormat(this ILog logger, LogLevel logLevel, string message, params object[] args)
        {
            logger.Log(logLevel, message.AsFunc(), null, args);
        }

        /// <summary>
        /// The Return.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="value">The value<see cref="T"/>.</param>
        /// <returns>The <see cref="T"/>.</returns>
        private static T Return<T>(this T value)
        {
            return value;
        }
    }
}
