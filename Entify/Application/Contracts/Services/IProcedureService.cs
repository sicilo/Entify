using System.Data.Common;

namespace Entify.Application.Contracts.Services;

public interface IProcedureService<T> where T : DbConnection
{
    Task ExecProcedureAsync(string storedProcedure);
    Task ExecProcedureAsync(string storedProcedure, params object[] parameters);
    Task<TR> ExecScalarProcedureAsync<TR>(string storedProcedure);
    Task<TR> ExecScalarProcedureAsync<TR>(string storedProcedure, params object[] parameters);
    Task<TR> ExecEntityProcedureAsync<TR>(string storedProcedure) where TR : class;
    Task<TR> ExecEntityProcedureAsync<TR>(string storedProcedure, params object[] parameters) where TR : class;
    Task<IEnumerable<TR>> ExecReaderProcedureAsync<TR>(string storedProcedure);
    Task<IEnumerable<TR>> ExecReaderProcedureAsync<TR>(string storedProcedure, params object[] parameters);
    Task<TR> ExecMultiReaderProcedureAsync<TR>(string storedProcedure) where TR : class;
    Task<TR> ExecMultiReaderProcedureAsync<TR>(string storedProcedure, params object[] parameters) where TR : class;
}