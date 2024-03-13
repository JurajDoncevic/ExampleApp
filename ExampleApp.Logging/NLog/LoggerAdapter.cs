
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace ExampleApp.Logging.NLog;

public class LoggerAdapter<T> : ILoggerAdapter<T>
{
    private readonly Microsoft.Extensions.Logging.ILogger<T> _logger;

    public LoggerAdapter(IConfiguration configuration)
    {
        _logger = LoggerFactory.Create(builder => builder.AddNLog(configuration))
                               .CreateLogger<T>();
    }

    public Func<bool> IsDebugEnabled => () => _logger.IsEnabled(LogLevel.Debug);

    public Func<bool> IsInfoEnabled => () => _logger.IsEnabled(LogLevel.Information);

    public Func<bool> IsWarnEnabled => () => _logger.IsEnabled(LogLevel.Warning);

    public Func<bool> IsErrorEnabled => () => _logger.IsEnabled(LogLevel.Error);

    public Func<bool> IsCriticalEnabled => () => _logger.IsEnabled(LogLevel.Critical);

    public void Critical(string message)
    {
        if (IsCriticalEnabled())
        {
            _logger.LogCritical(message);
        }
        
    }

    public void Critical<T1>(string messageTemplate, T1 arg1)
    {
        if (IsCriticalEnabled())
        {
            _logger.LogCritical(messageTemplate, arg1);
        }
        
    }

    public void Critical<T1, T2>(string messageTemplate, T1 arg1, T2 arg2)
    {
        if (IsCriticalEnabled())
        {
            _logger.LogCritical(messageTemplate, arg1, arg2);
        }
        
    }

    public void Critical<T1, T2, T3>(string messageTemplate, T1 arg1, T2 arg2, T3 arg3)
    {
        if (IsCriticalEnabled())
        {
            _logger.LogCritical(messageTemplate, arg1, arg2, arg3);
        }
        
    }

    public void Critical<T1, T2, T3, T4>(string messageTemplate, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        if (IsCriticalEnabled())
        {
            _logger.LogCritical(messageTemplate, arg1, arg2, arg3, arg4);
        }
        
    }

    public void Debug(string message)
    {
        if (IsDebugEnabled())
        {
            _logger.LogDebug(message);
        }
        
    }

    public void Debug<T1>(string messageTemplate, T1 arg1)
    {
        if (IsDebugEnabled())
        {
            _logger.LogDebug(messageTemplate, arg1);
        }
        
    }

    public void Debug<T1, T2>(string messageTemplate, T1 arg1, T2 arg2)
    {
        if (IsDebugEnabled())
        {
            _logger.LogDebug(messageTemplate, arg1, arg2);
        }
        
    }

    public void Debug<T1, T2, T3>(string messageTemplate, T1 arg1, T2 arg2, T3 arg3)
    {
        if (IsDebugEnabled())
        {
            _logger.LogDebug(messageTemplate, arg1, arg2, arg3);
        }
        
    }

    public void Debug<T1, T2, T3, T4>(string messageTemplate, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        if (IsDebugEnabled())
        {
            _logger.LogDebug(messageTemplate, arg1, arg2, arg3, arg4);
        }
        
    }

    public void Error(string message)
    {
        if (IsErrorEnabled())
        {
            _logger.LogError(message);
        }
        
    }

    public void Error<T1>(string messageTemplate, T1 arg1)
    {
        if (IsErrorEnabled())
        {
            _logger.LogError(messageTemplate, arg1);
        }
        
    }

    public void Error<T1, T2>(string messageTemplate, T1 arg1, T2 arg2)
    {
        if (IsErrorEnabled())
        {
            _logger.LogError(messageTemplate, arg1, arg2);
        }
        
    }

    public void Error<T1, T2, T3>(string messageTemplate, T1 arg1, T2 arg2, T3 arg3)
    {
        if (IsErrorEnabled())
        {
            _logger.LogError(messageTemplate, arg1, arg2, arg3);
        }
        
    }

    public void Error<T1, T2, T3, T4>(string messageTemplate, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        if (IsErrorEnabled())
        {
            _logger.LogError(messageTemplate, arg1, arg2, arg3, arg4);
        }
        
    }

    public void Info(string message)
    {
        if (IsInfoEnabled())
        {
            _logger.LogInformation(message);
        }
        
    }

    public void Info<T1>(string messageTemplate, T1 arg1)
    {
        if (IsInfoEnabled())
        {
            _logger.LogInformation(messageTemplate, arg1);
        }
        
    }

    public void Info<T1, T2>(string messageTemplate, T1 arg1, T2 arg2)
    {
        if (IsInfoEnabled())
        {
            _logger.LogInformation(messageTemplate, arg1, arg2);
        }
        
    }

    public void Info<T1, T2, T3>(string messageTemplate, T1 arg1, T2 arg2, T3 arg3)
    {
        if (IsInfoEnabled())
        {
            _logger.LogInformation(messageTemplate, arg1, arg2, arg3);
        }
        
    }

    public void Info<T1, T2, T3, T4>(string messageTemplate, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        if (IsInfoEnabled())
        {
            _logger.LogInformation(messageTemplate, arg1, arg2, arg3, arg4);
        }
        
    }

    public void Warn(string message)
    {
        if (IsWarnEnabled())
        {
            _logger.LogWarning(message);
        }
        
    }

    public void Warn<T1>(string messageTemplate, T1 arg1)
    {
        if (IsWarnEnabled())
        {
            _logger.LogWarning(messageTemplate, arg1);
        }
        
    }

    public void Warn<T1, T2>(string messageTemplate, T1 arg1, T2 arg2)
    {
        if (IsWarnEnabled())
        {
            _logger.LogWarning(messageTemplate, arg1, arg2);
        }
        
    }

    public void Warn<T1, T2, T3>(string messageTemplate, T1 arg1, T2 arg2, T3 arg3)
    {
        if (IsWarnEnabled())
        {
            _logger.LogWarning(messageTemplate, arg1, arg2, arg3);
        }
        
    }

    public void Warn<T1, T2, T3, T4>(string messageTemplate, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        if (IsWarnEnabled())
        {
            _logger.LogWarning(messageTemplate, arg1, arg2, arg3, arg4);
        }
        
    }
}
