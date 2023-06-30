namespace Entify.Utilities;

using Helpers;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

public static class DbProceduresExtensions
{
    /// <summary>
    /// executes a stored procedure in the database.
    /// </summary>
    /// <param name="connection">connection that implements DbConnection</param>
    /// <param name="storedProcedure">name of the stored procedure</param>
    /// <typeparam name="TResult">type of the result that is expected</typeparam>
    /// <returns>returns the first result of a generic IEnumerable</returns>
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

    /// <summary>
    /// executes a stored procedure in the database.
    /// </summary>
    /// <param name="connection">connection that implements DbConnection</param>
    /// <param name="storedProcedure">name of the stored procedure</param>
    /// <param name="parameters">stored procedure's parameters</param>
    /// <typeparam name="TResult">type of the result that is expected</typeparam>
    /// <returns>returns the first result of a generic IEnumerable</returns>
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

    /// <summary>
    /// executes a stored procedure in the database.
    /// </summary>
    /// <param name="connection">connection that implements DbConnection</param>
    /// <param name="storedProcedure">name of the stored procedure</param>
    /// <typeparam name="TResult">type of the result that is expected</typeparam>
    /// <returns>returns a generic IEnumerable</returns>
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

    /// <summary>
    /// executes a stored procedure with its parameters in the database.
    /// </summary>
    /// <param name="connection">connection that implements DbConnection</param>
    /// <param name="parameters">stored procedure's parameters</param>
    /// <param name="storedProcedure">name of the stored procedure</param>
    /// <typeparam name="TResult">type of the result that is expected</typeparam>
    /// <returns>returns a generic IEnumerable</returns>
    public static async Task<IEnumerable<TResult>> ExecProcListAsync<TResult>(this DbConnection connection,
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

    /// <summary>
    /// executes a stored procedure in the database.
    /// </summary>
    /// <param name="connection">connection that implements DbConnection</param>
    /// <param name="storedProcedure">name of the stored procedure</param>
    /// <typeparam name="TResult">type of the result that is expected</typeparam>
    /// <returns>returns just one single value</returns>
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

    /// <summary>
    /// executes a stored procedure with its parameters in the database.
    /// </summary>
    /// <param name="connection">connection that implements DbConnection</param>
    /// <param name="storedProcedure">name of the stored procedure</param>
    /// <param name="parameters">stored procedure's parameters</param>
    /// <typeparam name="TResult">type of the result that is expected</typeparam>
    /// <returns>returns just one single value</returns>
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
            await connection.OpenAsync();
            var result = await func(connection);
            await connection.CloseAsync();
            return result;
        }
        catch (Exception e)
        {
            throw new Exception("Entify process exception", e);
        }
    }
}