using Entify.Application.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Entify.Tests;

public class ListProcedures : TestBase
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task ExecProcListAsyncReturnsGenericList()
    {
        var parameters = Connection.ToDbParameters(new
        {
            Op = "READ"
        }).ToArray();

        var areas = (await Connection.ExecProcReaderAsync<Areas>("Sp_Areas", parameters)).ToArray();

        foreach (var area in areas)
        {
            Console.WriteLine(area);
        }

        Assert.IsTrue(areas.Any());
    }

    [Test]
    public async Task ExecProcFirstAsyncReturnsGenericList()
    {
        var parameters = Connection.ToDbParameters(new
        {
            Op = "READ"
        }).ToArray();

        var area = (await Connection.ExecProcEntityAsync<Areas>("Sp_Areas", parameters));

        Console.WriteLine(area);

        Assert.IsTrue(area is not null);
    }
}