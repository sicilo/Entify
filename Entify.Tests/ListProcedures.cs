namespace Entify.Tests;

using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using NUnit.Framework;

public class ListProcedures : TestBase
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task ExecProcListAsyncReturnsGenericList()
    {
        var areas = (await Connection.ExecProcListAsync<Areas>("Sp_Areas", new
        {
            Op = "READ"
        }.ToDbParameters())).ToArray();

        foreach (var area in areas)
        {
            Console.WriteLine(area);
        }

        Assert.IsTrue(areas.Any());
    }
}