using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Entify.Application.Extensions;
using NUnit.Framework;

namespace Entify.Tests;

public class MultiResultTest : TestBase
{
    [Test]
    public async Task ConsultPaginatedAreas()
    {
        var parameters = Connection.ToDbParameters(new
        {
            Op = "Paginated",
            Id = 2
        }).ToArray();
        
        var data = await Connection.ExecProcMultiReaderAsync<Paginated<Area>>("Sp_Areas", parameters);

        Console.WriteLine(data);
        foreach (var d in data.Data)
        {
            Console.WriteLine(d);
        }
    }
}

public record Area
{
    [Column("Id")]
    public int IdArea { get; init; }
    public string Codigo { get; init; } = string.Empty;
    public string Nombre { get; init; } = string.Empty;
}

public record Paginated<T>
{
    public int TotalRows { get; set; }
    
    public List<T> Data { get; set; }
}