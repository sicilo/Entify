namespace Entify.Services.Logger
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public static class EntifyLoggerExtensions
    {
        public static ILoggingBuilder AddEntifyLogger(this ILoggingBuilder builder, Action<EntifyLoggerOptions> configure)
        {
            builder.Services.AddSingleton<ILoggerProvider, EntifyLoggerProvider>();
            builder.Services.Configure(configure);
            return builder;
        }
    }
}
