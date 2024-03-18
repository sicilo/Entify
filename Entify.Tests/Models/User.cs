namespace Entify.Tests.Models;

public record User
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string Pass { get; init; } = null!;
    public DateTime Created { get; init; }
    public bool State { get; init; }
}