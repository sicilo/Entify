using System.Data;
using System.Data.Common;
using Entify.Application.Exceptions;
using Entify.Application.Helpers;
using Entify.Application.Resources;

namespace Entify.Application.Extensions;

public static class DbConnectionExtensions
{
    public static async Task<TResult> ExecProcScalarAsync<TResult>(
        this DbConnection connection,
        string storedProcedure,
        params DbParameter[] parameters
    )
        => await connection.Exec(storedProcedure,async command =>
        {
            command.Parameters.AddRange(parameters);

            var scalar = await command.ExecuteScalarAsync();

            if (scalar is null)
                throw new EntifyException(ExceptionMessages.NullReferenceException);

            return scalar.ConvertTo<TResult>();
        });

    public static async Task<TResult> ExecProcScalarAsync<TResult>(
        this DbConnection connection,
        string storedProcedure
    )
        => await connection.Exec(storedProcedure,async command =>
        {
            var scalar = await command.ExecuteScalarAsync();

            if (scalar is null)
                throw new EntifyException(ExceptionMessages.NullReferenceException);

            return scalar.ConvertTo<TResult>();
        });

    public static async Task<TResult> ExecProcMultiReaderAsync<TResult>(
        this DbConnection connection,
        string storedProcedure,
        params DbParameter[] parameters
    )
        => await connection.Exec(storedProcedure,async command =>
        {
            command.Parameters.AddRange(parameters);

            var reader = await command.ExecuteReaderAsync();

            return reader.MultiResultReaderToEntity<TResult>();
        });

    public static async Task<TResult> ExecProcMultiReaderAsync<TResult>(
        this DbConnection connection,
        string storedProcedure
    )
        => await connection.Exec(storedProcedure,async command =>
        {
            var reader = await command.ExecuteReaderAsync();

            return reader.MultiResultReaderToEntity<TResult>();
        });

    public static async Task<IEnumerable<TResult>> ExecProcReaderAsync<TResult>(
        this DbConnection connection,
        string storedProcedure,
        params DbParameter[] parameters
    )
        => await connection.Exec(storedProcedure,async command =>
        {
            command.Parameters.AddRange(parameters);

            var reader = await command.ExecuteReaderAsync();

            return reader.ReaderToList<TResult>();
        });

    public static async Task<IEnumerable<TResult>> ExecProcReaderAsync<TResult>(
        this DbConnection connection,
        string storedProcedure
    )
        => await connection.Exec(storedProcedure,async command =>
        {
            var reader = await command.ExecuteReaderAsync();

            return reader.ReaderToList<TResult>();
        });
    
    public static async Task<TResult> ExecProcEntityAsync<TResult>(
        this DbConnection connection,
        string storedProcedure,
        params DbParameter[] parameters
    )
        => await connection.Exec(storedProcedure,async command =>
        {
            command.Parameters.AddRange(parameters);

            var reader = await command.ExecuteReaderAsync();
            
            return reader.ReaderToEntity<TResult>();
        });
    
    public static async Task<TResult> ExecProcEntityAsync<TResult>(
        this DbConnection connection,
        string storedProcedure
    )
        => await connection.Exec(storedProcedure,async command =>
        {
            var reader = await command.ExecuteReaderAsync();

            return reader.ReaderToEntity<TResult>();
        });

    public static async Task ExecProcNonQuery(
        this DbConnection connection,
        string storedProcedure,
        params DbParameter[] parameters
    )
        => await connection.Exec(storedProcedure, async command =>
        {

            command.Parameters.AddRange(parameters);

            await command.ExecuteNonQueryAsync();
        });


    public static async Task ExecProcNonQuery(
        this DbConnection connection,
        string storedProcedure 
    )
        => await connection.Exec(storedProcedure,async command =>
        {
            await command.ExecuteNonQueryAsync();
        });


    private static async Task<TResult> Exec<TResult>(
        this DbConnection connection,
        string storedProcedure,
        Func<DbCommand,Task<TResult>> func)
    {
        try
        {
            var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = storedProcedure;
            
            await connection.OpenAsync();
            var result = await func(command);
            return result;
        }
        catch (Exception e)
        {
            throw new Exception(string.Format(ExceptionMessages.EntifyException, e.Message));
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    private static async Task Exec(
        this DbConnection connection,
        string storedProcedure,
        Func<DbCommand,Task> func)
    {
        try
        {
            var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = storedProcedure;
            
            await connection.OpenAsync();
            await func(command);
        }
        catch (Exception e)
        {
            throw new Exception(string.Format(ExceptionMessages.EntifyException, e.Message));
        }
        finally
        {
            await connection.CloseAsync();
        }
    }
}