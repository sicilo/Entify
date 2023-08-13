using System;
using System.Linq;
using System.Threading.Tasks;
using Entify.Application.Attributes;
using Entify.Application.Extensions;
using NUnit.Framework;

namespace Entify.Tests;

public class ParamClass {
    
    [ProcedureParameter("Op")]
    public string Opcion { get; set; }
}

public class ScalarProcedures : TestBase
{
    [Test]
    public async Task ExecProcScalarAsyncReturnsString()
    {
        var parameters = Connection.ToDbParameters(new ParamClass
        {
            Opcion = "STRING"
        }).ToArray();
        var text = await Connection.ExecProcScalarAsync<string>("SpScalarValues", parameters);

        Assert.IsTrue(text.Equals("Hola Mundo"));
    }

    [Test]
    public async Task ExecProcScalarAsyncReturnsInt()
    {
        var number = await Connection.ExecProcScalarAsync<int>("SpScalarValues", Connection.ToDbParameters(new
        {
            Op = "INT"
        }).ToArray());

        Assert.IsTrue(number.Equals(3));
    }
    
    [Test]
    public async Task ExecProcScalarAsyncReturnsDecimal()
    {
        var number = await Connection.ExecProcScalarAsync<decimal>("SpScalarValues", Connection.ToDbParameters(new
        {
            Op = "DECIMAL"
        }).ToArray());
        Console.WriteLine(number);
        Assert.IsTrue(number > 0);
    }
    
    [Test]
    public async Task ExecProcScalarAsyncReturnsFloat()
    {
        var number = await Connection.ExecProcScalarAsync<float>("SpScalarValues", Connection.ToDbParameters(new
        {
            Op = "FLOAT"
        }).ToArray());
        Console.WriteLine(number);
        Assert.IsTrue(number > 0);
    }

    [Test]
    public void ExecProcScalarAsyncReturnsException()
    {
        var ex = Assert.ThrowsAsync<Exception>(async () => await Connection.ExecProcScalarAsync<int>("SpScalarValues",
            Connection.ToDbParameters(new
            {
                Op = "STRING"
            }).ToArray()));

        if (ex is not null)
        {
            Console.WriteLine($"{ex.Message} -> {ex.InnerException?.Message}");
            Assert.IsTrue(ex.Message.Contains("Entify process exception"));
        }
    }
}