using Microsoft.Extensions.Configuration;

namespace ExampleApp.Logging;

public sealed class Logger : ILogger
{
    private readonly IConfiguration _configuration;

    public Logger(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ILoggerAdapter<T> ResolveLogger<T>()
    {
        return new NLog.LoggerAdapter<T>(_configuration);
    }
}
