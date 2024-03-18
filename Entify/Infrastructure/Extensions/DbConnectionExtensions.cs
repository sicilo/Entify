using System.Data;
using Entify.Application.Helpers;
using Entify.Domain.Exceptions;
using Entify.Domain.Resources;
using System.Data.Common;
using System.Globalization;

namespace Entify.Infrastructure.Extensions;

internal static class DbConnectionExtensions
{
    public static async Task<TResult> ExecScalarScriptAsync<TResult>(
        this DbConnection connection,
        string script,
        CommandType commandType = CommandType.Text,
        params object[] parameters
    )
        => await connection.ExecuteCommandAsync(script, async command =>
        {
            var @params = connection.ToDbParameters(parameters).ToArray();
            command.Parameters.AddRange(@params);
            command.CommandType = commandType;
            
            var scalar = await command.ExecuteScalarAsync();

            if (scalar is null)
                throw new EntifyException(ExceptionMessages.NullReferenceException);

            return scalar.ConvertTo<TResult>();
        });

    public static async Task<TResult> ExecScalarScriptAsync<TResult>(
        this DbConnection connection,
        string script,
        CommandType commandType = CommandType.Text
    )
        => await connection.ExecuteCommandAsync(script, async command =>
        {
            command.CommandType = commandType;
            var scalar = await command.ExecuteScalarAsync();

            if (scalar is null)
                throw new EntifyException(ExceptionMessages.NullReferenceException);

            return scalar.ConvertTo<TResult>();
        });

    public static async Task<TResult> ExecMultiReaderScriptAsync<TResult>(
        this DbConnection connection,
        string script,
        CommandType commandType = CommandType.Text,
        params object[] parameters
    )
        => await connection.ExecuteCommandAsync(script, async command =>
        {
            var @params = connection.ToDbParameters(parameters).ToArray();
            command.Parameters.AddRange(@params);
            command.CommandType = commandType;

            var reader = await command.ExecuteReaderAsync();

            return reader.MultiResultReaderToEntity<TResult>();
        });

    public static async Task<TResult> ExecMultiReaderScriptAsync<TResult>(
        this DbConnection connection,
        string script,
        CommandType commandType = CommandType.Text
    )
        => await connection.ExecuteCommandAsync(script, async command =>
        {
            command.CommandType = commandType;

            var reader = await command.ExecuteReaderAsync();

            return reader.MultiResultReaderToEntity<TResult>();
        });

    public static async Task<IEnumerable<TResult>> ExecReaderScriptAsync<TResult>(
        this DbConnection connection,
        string script,
        CommandType commandType = CommandType.Text,
        params object[] parameters
    )
        => await connection.ExecuteCommandAsync(script, async command =>
        {
            var @params = connection.ToDbParameters(parameters).ToArray();
            command.Parameters.AddRange(@params);
            command.CommandType = commandType;

            var reader = await command.ExecuteReaderAsync();

            return reader.ReaderToList<TResult>();
        });

    public static async Task<IEnumerable<TResult>> ExecReaderScriptAsync<TResult>(
        this DbConnection connection,
        string script,
        CommandType commandType = CommandType.Text
    )
        => await connection.ExecuteCommandAsync(script, async command =>
        {
            command.CommandType = commandType;

            var reader = await command.ExecuteReaderAsync();

            return reader.ReaderToList<TResult>();
        });

    public static async Task<TResult> ExecEntityScriptAsync<TResult>(
        this DbConnection connection,
        string script,
        CommandType commandType = CommandType.Text,
        params object[] parameters
    )
        => await connection.ExecuteCommandAsync(script, async command =>
        {
            var @params = connection.ToDbParameters(parameters).ToArray();
            command.Parameters.AddRange(@params);
            command.CommandType = commandType;

            var reader = await command.ExecuteReaderAsync();

            return reader.ReaderToEntity<TResult>();
        });

    public static async Task<TResult> ExecEntityScriptAsync<TResult>(
        this DbConnection connection,
        string script,
        CommandType commandType = CommandType.Text
    )
        => await connection.ExecuteCommandAsync(script, async command =>
        {
            command.CommandType = commandType;

            var reader = await command.ExecuteReaderAsync();

            return reader.ReaderToEntity<TResult>();
        });

    public static async Task ExecNonQueryScriptAsync(
        this DbConnection connection,
        string storedProcedure,
        CommandType commandType = CommandType.Text,
        params object[] parameters
    )
        => await connection.ExecuteCommandAsync(storedProcedure, async command =>
        {
            var @params = connection.ToDbParameters(parameters).ToArray();
            command.Parameters.AddRange(@params);
            command.CommandType = commandType;

            await command.ExecuteNonQueryAsync();
        });


    public static async Task ExecNonQueryScriptAsync(
        this DbConnection connection,
        string storedProcedure,
        CommandType commandType = CommandType.Text
    )
        => await connection.ExecuteCommandAsync(storedProcedure,
            async command =>
            {
                command.CommandType = commandType;

                await command.ExecuteNonQueryAsync();
            });


    private static async Task<TResult> ExecuteCommandAsync<TResult>(
        this DbConnection connection,
        string storedProcedure,
        Func<DbCommand, Task<TResult>> func)
    {
        try
        {
            var command = connection.CreateCommand();
            command.CommandText = storedProcedure;

            connection.Open();
            var result = await func(command);
            return result;
        }
        catch (Exception e)
        {
            var message = string.Format(
                CultureInfo.InvariantCulture,
                ExceptionMessages.EntifyException,
                nameof(ExecuteCommandAsync)
            );

            throw new Exception(message, e);
        }
        finally
        {
            connection.Close();
        }
    }

    private static async Task ExecuteCommandAsync(
        this DbConnection connection,
        string storedProcedure,
        Func<DbCommand, Task> func)
    {
        try
        {
            var command = connection.CreateCommand();
            command.CommandText = storedProcedure;

            connection.Open();
            await func(command);
        }
        catch (Exception e)
        {
            var message = string.Format(
                CultureInfo.InvariantCulture,
                ExceptionMessages.EntifyException,
                nameof(ExecuteCommandAsync)
            );

            throw new Exception(message, e);
        }
        finally
        {
            connection.Close();
        }
    }
}