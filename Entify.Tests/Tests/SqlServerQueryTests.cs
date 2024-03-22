using System.Net.Quic;
using Entify.Tests.Data;
using Entify.Tests.Models;
using Entify.Tests.Referentials;
using Xunit.Abstractions;

namespace Entify.Tests;

public sealed class SqlServerQueryTests : BaseSqlServerTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public SqlServerQueryTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [ClassData(typeof(QueryTestData))]
    public async Task ExecQueryAsync_SuccessTest(string userName, string pass)
    {
        //Arrange 
        var query =
            $"INSERT INTO Users(Name,Pass) " +
            $"VALUES ('{userName}','{pass}');";

        //Act
        var exception = await Record.ExceptionAsync(async () => await _queryService.ExecQueryAsync(query));


        //Assert
        if (exception is { InnerException: not null })
        {
            const string exceptedException = "UNIQUE KEY";
            Assert.Contains(exceptedException, exception.InnerException.Message);
        }
        else
        {
            Assert.Null(exception);
        }
    }

    [Theory]
    [InlineData("camilo.mejia")]
    public async Task ExecEntityQueryAsync_Success(string userName)
    {
        //Arrange 
        var query =
            $"SELECT Id, Name, Pass, Created, State FROM Users WHERE Name = '{userName}'";

        //Act
        var user = await _queryService.ExecEntityQueryAsync<User>(query);

        //Assert
        Assert.IsType<User>(user);
        Assert.Equal(userName, user.Name);

        _testOutputHelper.WriteLine(user.ToString());
    }


    [Fact]
    public async Task ExecScalarQueryAsync_ReturnsBool_Success()
    {
        //Arrange 
        const string query =
            "SELECT CASE WHEN EXISTS ( SELECT Id FROM Users WHERE Name = 'camilo.mejia') THEN 1 ELSE 0 END";

        //Act
        var existsUser = await _queryService.ExecScalarQueryAsync<bool>(query);

        //Assert
        Assert.IsType<bool>(existsUser);

        var existsText = existsUser ? "" : "not ";
        _testOutputHelper.WriteLine($"User {existsText}exists");
    }
    
    [Fact]
    public async Task ExecScalarQueryAsync_ReturnsString_Success()
    {
        //Arrange 
        const string query =
            "SELECT Id FROM Users WHERE Name = 'camilo.mejia'";

        //Act
        var userId = await _queryService.ExecScalarQueryAsync<Guid>(query);

        //Assert
        Assert.IsType<Guid>(userId);

        _testOutputHelper.WriteLine($"Id :{userId}");
    }
    
    [Fact]
    public async Task ExecScalarQueryAsync_ReturnsDateTime_Success()
    {
        //Arrange 
        const string query =
            "SELECT Created FROM Users WHERE Name = 'camilo.mejia'";

        //Act
        var userCreationDate = await _queryService.ExecScalarQueryAsync<DateTime>(query);

        //Assert
        Assert.IsType<DateTime>(userCreationDate);

        _testOutputHelper.WriteLine($"Id :{userCreationDate}");
    }

    [Fact]
    public async Task ExecReaderQueryAsync_ReturnsEnumerable_Success()
    {
        //Arrange
        const string query =
            "SELECT Id, Name, Pass, Created, State FROM Users";
        
        //Act
        var users = await _queryService.ExecReaderQueryAsync<User>(query);

        //Assert
        Assert.IsAssignableFrom<IEnumerable<User>>(users);

        foreach (var user in users)
        {
            _testOutputHelper.WriteLine(user.ToString());
        }
    }
    
    [Fact]
    public async Task ExecMultiReaderQueryAsync_ReturnsEnumerable_Success()
    {
        //Arrange
        const string query =
            @"  SELECT COUNT(Id) TotalPages FROM Users;
                SELECT COUNT(Id)/5 TotalPages FROM Users;
                SELECT Id, Name, Pass, Created, State FROM Users;";
        
        //Act
        var pagedUsers = await _queryService.ExecMultiReaderQueryAsync<Paged<User>>(query);

        //Assert
        Assert.IsAssignableFrom<Paged<User>>(pagedUsers);

        _testOutputHelper.WriteLine(pagedUsers.TotalPages.ToString());
        _testOutputHelper.WriteLine(pagedUsers.TotalRows.ToString());
        
        foreach (var user in pagedUsers.Items)
        {
            _testOutputHelper.WriteLine(user.ToString());
        }
    }
}

