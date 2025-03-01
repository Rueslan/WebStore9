using System.Collections.Concurrent;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace WebStore9.Logger;

public class Log4LoggerProvider: ILoggerProvider 
{
    private readonly string _configurationFile;

    private readonly ConcurrentDictionary<string, Log4NetLogger> _loggers = new();

    public Log4LoggerProvider(string configurationFile) => _configurationFile = configurationFile;

    public void Dispose() => _loggers.Clear();

    public ILogger CreateLogger(string categoryName) => _loggers.GetOrAdd(categoryName, category =>
    {
        var xml = new XmlDocument();
        xml.Load(_configurationFile);
        return new Log4NetLogger(category, xml["log4net"]);
    });
}