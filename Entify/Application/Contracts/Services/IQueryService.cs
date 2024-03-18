namespace Entify.Application.Contracts.Services
{
    using System.Data.Common;

    public interface IQueryService<T> where T : DbConnection
    {
        Task ExecQueryAsync(string query);
        Task<TR> ExecScalarQueryAsync<TR>(string query);
        Task<TR> ExecEntityQueryAsync<TR>(string query) where TR : class;
        Task<IEnumerable<TR>> ExecReaderQueryAsync<TR>(string query);
        Task<TR> ExecMultiReaderQueryAsync<TR>(string query) where TR : class;
    }
}
