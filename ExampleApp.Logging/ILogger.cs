namespace ExampleApp.Logging;

/// <summary>
/// Logger that resolves the logger implementation for a given type (calling class)
/// </summary>
public interface ILogger
{
    /// <summary>
    /// Resolves the logger implementation for a given type (calling class)
    /// </summary>
    public ILoggerAdapter<T> ResolveLogger<T>();
}
