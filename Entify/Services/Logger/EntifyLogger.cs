namespace Entify.Services.Logger
{
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;

    public class EntifyLogger : ILogger
    {
        private readonly EntifyLoggerProvider _provider;

        public EntifyLogger([NotNull] EntifyLoggerProvider provider)
        {
            _provider = provider;
            if(_provider.Options.Create)
                CreateLogStructs();
        }

        public IDisposable BeginScope<TState>(TState state) => default!;

        public bool IsEnabled(LogLevel logLevel)
            => logLevel != LogLevel.None;

        public void CreateLogStructs()
        {
            using SqlConnection connection = new(_provider.Options.ConnectionString);
            connection.Open();
            using SqlCommand command = new();
            command.Connection = connection;
            command.CommandType = CommandType.Text;
            command.CommandText = string.Format(SqlQueries.CREATE_TABLE, _provider.Options.LogTable);
            command.ExecuteNonQuery();
            command.CommandText = string.Format(SqlQueries.CREATE_SP, _provider.Options.SpLog, _provider.Options.LogTable);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            int threadId = Environment.CurrentManagedThreadId;

            using SqlConnection connection = new(_provider.Options.ConnectionString);
            connection.Open();

            JObject values = new();

            if (_provider.Options.LogFields.Any())
                foreach (string logField in _provider.Options.LogFields)
                    switch (logField)
                    {
                        case "LogLevel":
                            if (!string.IsNullOrWhiteSpace(logLevel.ToString()))
                                values.Add(logField, logLevel.ToString());
                            break;
                        case "ThreadId":
                            values.Add(logField, threadId);
                            break;
                        case "EventId":
                            values.Add(logField, eventId.Id);
                            break;
                        case "EventName":
                            values.Add(logField, eventId.Name);
                            break;
                        case "Message":
                            if (!string.IsNullOrWhiteSpace(formatter(state, exception)))
                                values.Add(logField, formatter(state, exception));
                            break;
                        case "ExceptionMessage":
                            if (exception != null && !string.IsNullOrWhiteSpace(exception.Message))
                                values.Add(logField, exception.Message);
                            break;
                        case "ExceptionStackTrace":
                            if (exception != null && !string.IsNullOrWhiteSpace(exception.StackTrace))
                                values.Add(logField, exception.StackTrace);
                            break;
                        case "ExceptionSource":
                            if (exception != null && !string.IsNullOrWhiteSpace(exception.Source))
                                values.Add(logField, exception.Source);
                            break;
                    }
                

            using SqlCommand command = new(SqlQueries.DEFAULT_PROC_NAME);

            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter(SqlQueries.OP,SqlQueries.OP_VALUE));
            command.Parameters.Add(new SqlParameter(SqlQueries.VALUE, JsonConvert.SerializeObject(values, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                Formatting = Formatting.None,
            })));
            command.Parameters.Add(new SqlParameter(SqlQueries.CREATED, DateTime.Now));

            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
