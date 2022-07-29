namespace Entify.Services.Logger
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    [ProviderAlias("EntityLogger")]
    public class EntifyLoggerProvider : ILoggerProvider
    {
        public readonly EntifyLoggerOptions Options;
        public EntifyLoggerProvider(IOptions<EntifyLoggerOptions> options)
        {
            Options = options.Value;
        }
        public ILogger CreateLogger(string categoryName)
            => new EntifyLogger(this);
        

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
