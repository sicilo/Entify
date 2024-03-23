﻿using Entify.Application.Contracts.Services;
using Entify.Application.Services;
using Microsoft.Data.SqlClient;

namespace Entify.Tests.Referentials;

public class BaseSqlServerTests 
{
    protected const string ConnectionString = "Data Source=localhost;Initial Catalog=Entify;User Id=sa;Password=P70m3t30;TrustServerCertificate=True;";

    protected readonly IProcedureService<SqlConnection> _procedureService;

    protected BaseSqlServerTests()
    {
    }
}