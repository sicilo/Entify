using System.Data.Common;

namespace Entify.Application.Contracts.Services;

public interface IProcedureService<T> where T : DbConnection
{
    Task ExecProcedureAsync();
    Task ExecProcedureAsync(params object[] parameters);
    Task<TR> ExecScalarProcedureAsync<TR>();
    Task<TR> ExecScalarProcedureAsync<TR>(params object[] parameters);
    Task<TR> ExecEntityProcedureAsync<TR>() where TR : class;
    Task<TR> ExecEntityProcedureAsync<TR>(params object[] parameters) where TR : class;
    Task<IEnumerable<TR>> ExecReaderProcedureAsync<TR>();
    Task<IEnumerable<TR>> ExecReaderProcedureAsync<TR>(params object[] parameters);
    Task<TR> ExecMultiReaderProcedureAsync<TR>() where TR : class;
    Task<TR> ExecMultiReaderProcedureAsync<TR>(params object[] parameters) where TR : class;
}