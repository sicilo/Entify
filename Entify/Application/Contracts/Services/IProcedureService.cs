using System.Data.Common;

namespace Entify.Application.Contracts.Services;

public interface IProcedureService<T> where T : DbConnection
{
    Task ExecProcedureAsync();
    Task<TR> ExecScalarProcedureAsync<TR>();
    Task<TR> ExecEntityProcedureAsync<TR>() where TR : class;
    Task<IEnumerable<TR>> ExecReaderProcedureAsync<TR>();
    Task<TR> ExecMultiReaderProcedureAsync<TR>() where TR : class;
}