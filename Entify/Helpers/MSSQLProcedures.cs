namespace Entify.Helpers.MSSQLProcedures
{
    using Entify.Utilities.Extensions;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    public static class MSSQLProcedures
    {
        public static async Task<Result> ExecProcFirstAsync<Result>(this SqlConnection connection, string storedProcedure)
        {
            return await ExecuteConnectionAsync(connection, async (SqlConnection dbConnection) =>
            {
                SqlCommand command = new(storedProcedure)
                {
                    Connection = connection,
                    CommandType = CommandType.StoredProcedure,
                };
                return (await command.ExecuteReader().ReaderToList<Result>()).First();
            });
        }

        public static async Task<Result> ExecProcFirstAsync<Result>(this SqlConnection connection, string storedProcedure, SqlParameter[] parameters)
        {
            return await ExecuteConnectionAsync(connection, async (SqlConnection dbConnection) =>
            {
                SqlCommand command = new(storedProcedure)
                {
                    Connection = connection,
                    CommandType = CommandType.StoredProcedure,
                };

                command.Parameters.AddRange(parameters);

                return (await command.ExecuteReader().ReaderToList<Result>()).First();
            });
        }

        public static async Task<List<Result>> ExecProcListAsync<Result>(this SqlConnection connection, string storedProcedure)
        {
            return await ExecuteConnectionAsync(connection, async (SqlConnection dbConnection) =>
            {
                SqlCommand command = new(storedProcedure)
                {
                    Connection = dbConnection,
                    CommandType = CommandType.StoredProcedure,
                };

                return await (await command.ExecuteReaderAsync()).ReaderToList<Result>();
            });
        }

        public static async Task<List<Result>> ExecProcListAsync<Result>(this SqlConnection connection, string storedProcedure, SqlParameter[] parameters)
        {
            return await ExecuteConnectionAsync(connection, async (SqlConnection dbConnection) =>
            {
                SqlCommand command = new(storedProcedure)
                {
                    Connection = dbConnection,
                    CommandType = CommandType.StoredProcedure,
                };

                command.Parameters.AddRange(parameters);

                return await (await command.ExecuteReaderAsync()).ReaderToList<Result>();
            });
        }

        public static async Task<Result> ExecProcScalarAsync<Result>(this SqlConnection connection, string storedProcedure)
        {
            return await ExecuteConnectionAsync(connection, async (SqlConnection dbConnection) =>
            {

                SqlCommand command = new(storedProcedure)
                {
                    Connection = dbConnection,
                    CommandType = CommandType.StoredProcedure,
                };

                return await Task.FromResult((Result)Convert.ChangeType(value: command.ExecuteScalar(), typeof(Result)));
            });
        }

        public static async Task<Result> ExecProcScalarAsync<Result>(this SqlConnection connection, string storedProcedure, SqlParameter[] parameters)
        {
            return await ExecuteConnectionAsync(connection, async (SqlConnection dbConnection) =>
            {
                SqlCommand command = new(storedProcedure)
                {
                    Connection = dbConnection,
                    CommandType = CommandType.StoredProcedure,

                };
                command.Parameters.AddRange(parameters);

                return await Task.FromResult((Result)Convert.ChangeType(value: command.ExecuteScalar(), typeof(Result)));
            });
        }


        private static async Task<Result> ExecuteConnectionAsync<Result>(this SqlConnection connection, Func<SqlConnection, Task<Result>> func)
        {
            connection.Open();
            Result result = await func(connection);
            connection.Close();
            return result;
        }
    }
}
