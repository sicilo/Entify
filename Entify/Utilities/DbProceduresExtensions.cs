namespace Entify.Utilities
{
    using Helpers;
    using System.Data;
    using System.Data.Common;
    using System.Threading.Tasks;

    public static class DbProceduresExtensions
    {
        public static async Task<TResult> ExecProcFirstAsync<TResult>(this DbConnection connection,
            string storedProcedure)
        {
            return await connection.ExecuteConnectionAsync(async dbConnection =>
            {
                var command = dbConnection.CreateCommand();
                command.Connection = dbConnection;
                command.CommandText = storedProcedure;
                command.CommandType = CommandType.StoredProcedure;

                return (await (await command.ExecuteReaderAsync()).ReaderToList<TResult>()).First();
            });
        }

        public static async Task<TResult> ExecProcFirstAsync<TResult>(this DbConnection connection,
            string storedProcedure, DbParameter[] parameters)
        {
            return await connection.ExecuteConnectionAsync(async dbConnection =>
            {
                var command = dbConnection.CreateCommand();
                command.Connection = dbConnection;
                command.CommandText = storedProcedure;
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddRange(parameters);

                return (await (await command.ExecuteReaderAsync()).ReaderToList<TResult>()).First();
            });
        }

        public static async Task<List<TResult>> ExecProcListAsync<TResult>(this DbConnection connection,
            string storedProcedure)
        {
            return await connection.ExecuteConnectionAsync(async dbConnection =>
            {
                var command = dbConnection.CreateCommand();
                command.Connection = dbConnection;
                command.CommandText = storedProcedure;
                command.CommandType = CommandType.StoredProcedure;

                return await (await command.ExecuteReaderAsync()).ReaderToList<TResult>();
            });
        }

        public static async Task<List<TResult>> ExecProcListAsync<TResult>(this DbConnection connection,
            string storedProcedure, DbParameter[] parameters)
        {
            return await connection.ExecuteConnectionAsync(async dbConnection =>
            {
                var command = dbConnection.CreateCommand();
                command.Connection = dbConnection;
                command.CommandText = storedProcedure;
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddRange(parameters);

                return await (await command.ExecuteReaderAsync()).ReaderToList<TResult>();
            });
        }

        public static async Task<TResult> ExecProcScalarAsync<TResult>(this DbConnection connection,
            string storedProcedure)
        {
            return await connection.ExecuteConnectionAsync(async dbConnection =>
            {
                var command = dbConnection.CreateCommand();
                command.Connection = dbConnection;
                command.CommandText = storedProcedure;
                command.CommandType = CommandType.StoredProcedure;

                var commandResult = await command.ExecuteScalarAsync();

                commandResult.ThrowsNullReferenceException();

                return commandResult.ConvertToGeneric<TResult>();
            });
        }

        public static async Task<TResult> ExecProcScalarAsync<TResult>(this DbConnection connection,
            string storedProcedure, DbParameter[] parameters)
        {
            return await connection.ExecuteConnectionAsync(async dbConnection =>
            {
                var command = dbConnection.CreateCommand();
                command.Connection = dbConnection;
                command.CommandText = storedProcedure;
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddRange(parameters);

                var commandResult = await command.ExecuteScalarAsync();

                return commandResult.ConvertToGeneric<TResult>();
            });
        }


        private static async Task<TResult> ExecuteConnectionAsync<TResult>(this DbConnection connection,
            Func<DbConnection, Task<TResult>> func)
        {
            try
            {
                connection.Open();
                var result = await func(connection);
                connection.Close();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Entify process exception", e);
            }
        }
    }
}