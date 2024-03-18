using System.Collections;

namespace Entify.Tests.Data;

public class QueryTestData : IEnumerable<object[]>
{
    private static readonly Random Random = new();

    private static readonly List<string> Names =
        ["Juan", "María", "Luis", "Ana", "Carlos", "Laura", "Pedro", "Sofía", "Jorge", "Elena"];

    private static readonly List<string> Surnames =
        ["García", "Martínez", "López", "González", "Rodríguez", "Fernández", "Pérez", "Gómez", "Sánchez", "Díaz"];

    public IEnumerator<object[]> GetEnumerator()
    {
        const int usersToCreate = 5;
        for (var i = 0; i < usersToCreate; i++)
        {
            var name = Names[Random.Next(Names.Count)];
            var surname = Surnames[Random.Next(Surnames.Count)];
            yield return [$"{name}.{surname}".ToLower(), surname];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}