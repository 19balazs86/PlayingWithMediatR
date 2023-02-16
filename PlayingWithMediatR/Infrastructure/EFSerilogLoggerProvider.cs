namespace PlayingWithMediatR.Infrastructure;

public sealed class EFSerilogLoggerProvider : ILoggerProvider
{
    public static readonly LoggerFactory LoggerFactory = new LoggerFactory(new[] { new EFSerilogLoggerProvider() });

    public ILogger CreateLogger(string categoryName)
    {
        return new EFSerilogLogger();
    }

    public void Dispose()
    { }

    private class EFSerilogLogger : ILogger
    {
        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Serilog.Log.Information(formatter(state, exception));
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}
