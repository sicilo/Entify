using System.Collections;
using Entify.Tests.Models;

namespace Entify.Tests.Data;

public class UsersTestData : IEnumerable<object[]>
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
    
    public IEnumerable<User> GetUsers(int usersToCreate = 5)
    {
        for (var i = 0; i < usersToCreate; i++)
        {
            var name = Names[Random.Next(Names.Count)];
            var surname = Surnames[Random.Next(Surnames.Count)];
            yield return new User
            {
                Name = $"{name}.{surname}",
                Pass = $"{surname}&{name}"
            };
        }
        
        yield return new User
        {
            Name = $"sicilo",
            Pass = $"sicilo"
        };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}