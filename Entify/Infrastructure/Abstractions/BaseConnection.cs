namespace Entify.Infrastructure.Abstractions
{
    using Entify.Domain.Exceptions;
    using System.Data.Common;
    using System.Globalization;

    public abstract class BaseConnection<T> where T : DbConnection
    {
        private readonly T _connection;

        protected BaseConnection(T connection)
        {
            _connection = connection;
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

            var connection = (T?)Activator.CreateInstance(_connection.GetType(), _connection.ConnectionString);

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