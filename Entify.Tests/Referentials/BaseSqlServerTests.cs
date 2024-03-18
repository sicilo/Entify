using Entify.Application.Contracts.Services;
using Entify.Application.Services;
using Microsoft.Data.SqlClient;

namespace Entify.Tests.Referentials;

public class BaseSqlServerTests
{
    private const string ConnectionString = "Data Source=localhost;Initial Catalog=Entify;User Id=sa;Password=P70m3t30;TrustServerCertificate=True;";

    protected readonly IQueryService<SqlConnection> _queryService;

    protected BaseSqlServerTests()
    {
        _queryService = new QueryService<SqlConnection>(ConnectionString);
    }
}