using System.Collections;

namespace Entify.Tests.Models;

public record Paged<T>
{
    public int TotalPages { get; set; }
    
    public int TotalRows { get; set; }

    public IEnumerable<T> Items { get; set; } = null!;
}