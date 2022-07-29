namespace Entify.Services.Logger
{
    public class EntifyLoggerOptions
    {
        public bool Create { get; set; } = false;
        public string ConnectionString { get; set; } = string.Empty;

        public string[] LogFields { get; set; } =
        {
            "LogLevel"
            ,"ThreadId"
            ,"EventId"
            ,"EventName"
            ,"Message"
            ,"ExceptionMessage"
            ,"ExceptionStackTrace"
            ,"ExceptionSource"
        };

        public string LogTable { get; set; } = SqlQueries.DEFAULT_TABLE_NAME;
        public string SpLog { get; set; } = SqlQueries.DEFAULT_PROC_NAME;

        public EntifyLoggerOptions()
        {
        }
    }
}
