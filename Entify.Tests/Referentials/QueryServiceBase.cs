using Entify.Application.Contracts.Services;
using Entify.Application.Services;
using Microsoft.Data.SqlClient;

namespace Entify.Tests.Referentials;

public class QueryServiceBase : ConnectionBase
{
    public readonly IQueryService<SqlConnection> QueryService;

    public QueryServiceBase()
    {
        QueryService = new QueryService<SqlConnection>(Connection);
    }
}