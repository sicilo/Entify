using System;
using System.Threading.Tasks;
using Entify.Utilities;
using NUnit.Framework;

namespace Entify.Tests;

public class ScalarProcedures : TestBase
{
    
    [Test]
    public async Task ExecProcScalarAsyncReturnsString()
    {
        var text = await Connection.ExecProcScalarAsync<string>("SpScalarValues", new
        {
            Op = "STRING"
        }.ToDbParameters());
        
        Assert.IsTrue(text.Equals("Hola Mundo"));
    }
    
    [Test]
    public async Task ExecProcScalarAsyncReturnsInt()
    {
        var number = await Connection.ExecProcScalarAsync<int>("SpScalarValues", new
        {
            Op = "INT"
        }.ToDbParameters());
        
        Assert.IsTrue(number.Equals(3));
    }
    
    [Test]
    public void ExecProcScalarAsyncReturnsException()
    {
        var ex = Assert.ThrowsAsync<Exception>(async () => await Connection.ExecProcScalarAsync<int>("SpScalarValues", new
        {
            Op = "STRING"
        }.ToDbParameters()));

        if (ex is not null) Console.WriteLine($"{ex.Message} -> {ex.InnerException?.Message}");
        
        Assert.IsTrue(ex.Message.Contains("Entify process exception"));
    }
}