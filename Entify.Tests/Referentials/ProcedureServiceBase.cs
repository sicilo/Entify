using Entify.Application.Contracts.Services;
using Entify.Application.Services;
using Microsoft.Data.SqlClient;

namespace Entify.Tests.Referentials;

public class ProcedureServiceBase : ConnectionBase
{
    public readonly IProcedureService<SqlConnection> ProcedureService;

    public ProcedureServiceBase()
    {
        ProcedureService = new ProcedureService<SqlConnection>(Connection);
    }
}