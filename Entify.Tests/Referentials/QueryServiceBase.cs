using Entify.Application.Contracts.Services;
using Entify.Application.Services;
using Microsoft.Data.SqlClient;

namespace Entify.Tests.Referentials;

public class QueryServiceBase : ConnectionBase
{
    public readonly IQueryService<SqlConnection> QueryService = new QueryService<SqlConnection>(ConnectionString);
}