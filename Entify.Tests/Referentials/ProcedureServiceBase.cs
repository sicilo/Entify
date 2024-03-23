using Entify.Application.Contracts.Services;
using Entify.Application.Services;
using Microsoft.Data.SqlClient;

namespace Entify.Tests.Referentials;

public class QueryServiceBase 
{
    protected const string ConnectionString = "Data Source=localhost;Initial Catalog=Entify;User Id=sa;Password=P70m3t30;TrustServerCertificate=True;";

    protected readonly IProcedureService<SqlConnection> _procedureService;

    protected QueryServiceBase()
    {
    }
}