using System.Reflection;
using Microsoft.Extensions.Logging;

namespace WebStore9.Logger;

public static class Log4NetLoggerFactoryExtensions
{
    public static string CheckFilePath(string filePath)
    {
        if (filePath is not { Length: > 0 })
            throw new ArgumentException("Не указан путь к файлу конфигурации log4net.config");

        if (Path.IsPathRooted(filePath))
            return filePath;

        var assembly = Assembly.GetEntryAssembly();
        var dir = Path.GetDirectoryName(assembly!.Location);

        return Path.Combine(dir!, filePath);
    }

    public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, string configurationFile = "log4net.config")
    {
        factory.AddProvider(new Log4LoggerProvider(CheckFilePath(configurationFile)));

        return factory;
    }
}