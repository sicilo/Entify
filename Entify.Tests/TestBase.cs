namespace Entify.Tests;

using NUnit.Framework;
using System.Data.SqlClient;

[TestFixture]
public class TestBase
{
    protected readonly SqlConnection Connection;
    
    public TestBase()
    {
        const string connectionString = "Data Source=localhost;Initial Catalog=Agree;Persist Security Info=False;User ID=sa;Password=P4nd4C07n10";

        Connection = new SqlConnection(connectionString);
    }
}