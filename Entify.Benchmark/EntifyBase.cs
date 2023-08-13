using System.Data.SqlClient;

namespace Entify.Benchmark;

public class EntifyBase
{
    protected readonly SqlConnection Connection;
    
    public EntifyBase()
    {
        const string connectionString = "Data Source=localhost;Initial Catalog=Agree;Persist Security Info=False;User ID=sa;Password=P4nd4C07n10";

        Connection = new SqlConnection(connectionString);
    }
}