namespace Entify.Application.Contracts.Services
{
    public interface IQueryService
    {
        Task ExecQueryAsync(string query);
        Task<TR> ExecScalarQueryAsync<TR>(string query);
        Task<TR> ExecEntityQueryAsync<TR>(string query) where TR : class;
        Task<IEnumerable<TR>> ExecReaderQueryAsync<TR>(string query);
        Task<TR> ExecMultiReaderQueryAsync<TR>(string query) where TR : class;
    }
}
