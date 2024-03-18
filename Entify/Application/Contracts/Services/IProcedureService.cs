namespace Entify.Application.Contracts.Services;

public interface IProcedureService
{
    Task ExecProcedureAsync();
    Task<TR> ExecScalarProcedureAsync<TR>();
    Task<TR> ExecEntityProcedureAsync<TR>() where TR : class;
    Task<IEnumerable<TR>> ExecReaderProcedureAsync<TR>();
    Task<TR> ExecMultiReaderProcedureAsync<TR>() where TR : class;
}