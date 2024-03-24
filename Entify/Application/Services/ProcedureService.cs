using System.Data;
using System.Data.Common;
using Entify.Application.Contracts.Services;
using Entify.Infrastructure.Abstractions;
using Entify.Infrastructure.Extensions;

namespace Entify.Application.Services;

public sealed class ProcedureService<T> : BaseConnection<T>, IProcedureService<T> where T : DbConnection
{
    private const CommandType ProcedureType = CommandType.StoredProcedure;

    public ProcedureService(T connection) : base(connection)
    {
    }
    
    public async Task ExecProcedureAsync(string storedProcedure)
    {
        await using var connection = CreateConnectionInstance();

        await connection.ExecNonQueryScriptAsync(storedProcedure);
    }

    public async Task ExecProcedureAsync(string storedProcedure,params object[] parameters)
    {
        await using var connection = CreateConnectionInstance();

        await connection.ExecNonQueryScriptAsync(storedProcedure, ProcedureType, parameters);
    }

    public async Task<TR> ExecScalarProcedureAsync<TR>(string storedProcedure)
    {
        await using var connection = CreateConnectionInstance();

        return await connection.ExecScalarScriptAsync<TR>(storedProcedure);
    }

    public async Task<TR> ExecScalarProcedureAsync<TR>(string storedProcedure,params object[] parameters)
    {
        await using var connection = CreateConnectionInstance();

        return await connection.ExecScalarScriptAsync<TR>(storedProcedure, ProcedureType, parameters);
    }

    public async Task<TR> ExecEntityProcedureAsync<TR>(string storedProcedure) where TR : class
    {
        await using var connection = CreateConnectionInstance();

        return await connection.ExecEntityScriptAsync<TR>(storedProcedure);
    }

    public async Task<TR> ExecEntityProcedureAsync<TR>(string storedProcedure,params object[] parameters) where TR : class
    {
        await using var connection = CreateConnectionInstance();

        return await connection.ExecEntityScriptAsync<TR>(storedProcedure, ProcedureType, parameters);
    }

    public async Task<IEnumerable<TR>> ExecReaderProcedureAsync<TR>(string storedProcedure)
    {
        await using var connection = CreateConnectionInstance();

        return await connection.ExecReaderScriptAsync<TR>(storedProcedure);
    }

    public async Task<IEnumerable<TR>> ExecReaderProcedureAsync<TR>(string storedProcedure,params object[] parameters)
    {
        await using var connection = CreateConnectionInstance();

        return await connection.ExecReaderScriptAsync<TR>(storedProcedure, ProcedureType, parameters);
    }

    public async Task<TR> ExecMultiReaderProcedureAsync<TR>(string storedProcedure) where TR : class
    {
        await using var connection = CreateConnectionInstance();

        return await connection.ExecMultiReaderScriptAsync<TR>(storedProcedure);
    }

    public async Task<TR> ExecMultiReaderProcedureAsync<TR>(string storedProcedure,params object[] parameters) where TR : class
    {
        await using var connection = CreateConnectionInstance();

        return await connection.ExecMultiReaderScriptAsync<TR>(storedProcedure, ProcedureType,
            parameters);
    }
}