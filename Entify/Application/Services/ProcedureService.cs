using System.Data;
using System.Data.Common;
using Entify.Application.Contracts.Services;
using Entify.Infrastructure.Abstractions;
using Entify.Infrastructure.Extensions;

namespace Entify.Application.Services;

public sealed class ProcedureService<T> : BaseConnection<T>, IProcedureService<T> where T : DbConnection
{
    private const CommandType ProcedureType = CommandType.StoredProcedure;
    private readonly string _storedProcedure;

    public ProcedureService(string storedProcedure, string connectionString) : base(connectionString)
    {
        _storedProcedure = storedProcedure;
    }

    public async Task ExecProcedureAsync()
    {
        await using var connection = CreateConnectionInstance();

        await connection.ExecNonQueryScriptAsync(_storedProcedure);
    }

    public async Task ExecProcedureAsync(params object[] parameters)
    {
        await using var connection = CreateConnectionInstance();

        await connection.ExecNonQueryScriptAsync(_storedProcedure, ProcedureType, parameters);
    }

    public async Task<TR> ExecScalarProcedureAsync<TR>()
    {
        await using var connection = CreateConnectionInstance();

        return await connection.ExecScalarScriptAsync<TR>(_storedProcedure);
    }

    public async Task<TR> ExecScalarProcedureAsync<TR>(params object[] parameters)
    {
        await using var connection = CreateConnectionInstance();

        return await connection.ExecScalarScriptAsync<TR>(_storedProcedure, ProcedureType, parameters);
    }

    public async Task<TR> ExecEntityProcedureAsync<TR>() where TR : class
    {
        await using var connection = CreateConnectionInstance();

        return await connection.ExecEntityScriptAsync<TR>(_storedProcedure);
    }

    public async Task<TR> ExecEntityProcedureAsync<TR>(params object[] parameters) where TR : class
    {
        await using var connection = CreateConnectionInstance();

        return await connection.ExecEntityScriptAsync<TR>(_storedProcedure, ProcedureType, parameters);
    }

    public async Task<IEnumerable<TR>> ExecReaderProcedureAsync<TR>()
    {
        await using var connection = CreateConnectionInstance();

        return await connection.ExecReaderScriptAsync<TR>(_storedProcedure);
    }

    public async Task<IEnumerable<TR>> ExecReaderProcedureAsync<TR>(params object[] parameters)
    {
        await using var connection = CreateConnectionInstance();

        return await connection.ExecReaderScriptAsync<TR>(_storedProcedure, ProcedureType, parameters);
    }

    public async Task<TR> ExecMultiReaderProcedureAsync<TR>() where TR : class
    {
        await using var connection = CreateConnectionInstance();

        return await connection.ExecMultiReaderScriptAsync<TR>(_storedProcedure);
    }

    public async Task<TR> ExecMultiReaderProcedureAsync<TR>(params object[] parameters) where TR : class
    {
        await using var connection = CreateConnectionInstance();

        return await connection.ExecMultiReaderScriptAsync<TR>(_storedProcedure, ProcedureType,
            parameters);
    }
}