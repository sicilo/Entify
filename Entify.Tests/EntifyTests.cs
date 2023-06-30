namespace Entify.Tests;

using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using NUnit.Framework;

[TestFixture]
public class EntifyTests
{
    private readonly SqlConnection _connection;
    public EntifyTests()
    {
        const string connectionString = "Data Source=localhost;Initial Catalog=Agree;Persist Security Info=False;User ID=sa;Password=P4nd4C07n10";

        _connection = new SqlConnection(connectionString);
    }
    
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public async Task ExecProcListAsyncReturnsGenericValue()
    {
        var areas = await _connection.ExecProcListAsync<Areas>("Sp_Areas",new
        {
            Op = "READ"
        }.ToDbParameters());

        foreach (var area in areas)
        {
            Console.WriteLine(area);
        }
        
        Assert.IsTrue(areas.Any());
    }
}