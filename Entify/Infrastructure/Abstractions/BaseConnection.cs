namespace Entify.Infrastructure.Abstractions
{
    using Entify.Domain.Exceptions;
    using System.Data.Common;
    using System.Globalization;

    public abstract class BaseConnection<T> where T : DbConnection
    {
        private readonly string _connectionString;

        protected BaseConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected T CreateConnectionInstance()
        {
            var connectionType = typeof(T);

            if (connectionType is null)
            {
                var message = string.Format(
                    CultureInfo.InvariantCulture,
                    "No connection type was given"
                    );

                throw new EntifyException(message);
            }

            var connection = (T?)Activator.CreateInstance(connectionType, _connectionString);

            if (connection is not null) return connection;
            {
                var message = string.Format(
                    CultureInfo.InvariantCulture,
                    $"We could not create an object connection of type {connectionType.Name}"
                );

                throw new EntifyException(message);
            }

        }
    }
}
