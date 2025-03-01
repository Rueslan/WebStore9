using log4net;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Reflection;
using System.Xml;

namespace WebStore9.Logger
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _log;

        public Log4NetLogger(string categoryName, XmlElement configuration)
        {
            var loggerRepository = LogManager
                .CreateRepository(
                    Assembly.GetEntryAssembly(),
                    typeof(log4net.Repository.Hierarchy.Hierarchy)
                );

            _log = LogManager.GetLogger(loggerRepository.Name, categoryName);

            log4net.Config.XmlConfigurator.Configure(configuration);
        }

        public IDisposable? BeginScope<TState>(TState state) => null;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (formatter is null)
                throw new ArgumentNullException(nameof(formatter));

            if (!IsEnabled(logLevel))
                return;

            var logString = formatter(state, exception);

            if (string.IsNullOrEmpty(logString) && exception is null)
                return;
            switch (logLevel)
            {
                default:
                    throw new InvalidEnumArgumentException(nameof(logLevel), (int)logLevel, typeof(LogLevel));

                case LogLevel.None:
                    break;

                case LogLevel.Trace:
                case LogLevel.Debug:
                    _log.Debug(logString);
                    break;

                case LogLevel.Information:
                    _log.Info(logString);
                    break;

                case LogLevel.Warning:
                    _log.Warn(logString);
                    break;

                case LogLevel.Error:
                    _log.Error(logString, exception);
                    break;

                case LogLevel.Critical:
                    _log.Fatal(logString);
                    break;


            }
        }

        public bool IsEnabled(LogLevel logLevel) => logLevel switch
        {
            LogLevel.None => false,
            LogLevel.Trace => _log.IsDebugEnabled,
            LogLevel.Debug => _log.IsDebugEnabled,
            LogLevel.Information => _log.IsInfoEnabled,
            LogLevel.Warning => _log.IsWarnEnabled,
            LogLevel.Error => _log.IsErrorEnabled,
            LogLevel.Critical => _log.IsFatalEnabled,
            _ => throw new InvalidEnumArgumentException(nameof(logLevel), (int)logLevel, typeof(LogLevel))
        };
    }
}
