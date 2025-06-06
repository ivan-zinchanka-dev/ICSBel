using Microsoft.Extensions.Logging;

namespace ICSBel.Tests.Utilities;

internal static class LoggingUtility
{
    public static ILogger<T> CreateLogger<T>()
    {
        return LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<T>();
    }
}