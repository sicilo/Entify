using Entify.Application.Contracts.Services;
using Entify.Application.Services;
using Entify.Tests.Data;
using Entify.Tests.Models;
using Entify.Tests.Referentials;
using Microsoft.Data.SqlClient;
using Xunit.Abstractions;

namespace Entify.Tests;

public class SqlServerProcedureTests : IClassFixture<ProcedureServiceBase>
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly IProcedureService<SqlConnection> _procedureService;

    public SqlServerProcedureTests(ITestOutputHelper testOutputHelper, ProcedureServiceBase procedureServiceBase)
    {
        _testOutputHelper = testOutputHelper;
        _procedureService = procedureServiceBase.ProcedureService;
    }
    //
    // [Fact]
    // public async Task Create()
    // {
    //     //Arrange
    //     
    //     //Act
    //     
    //     //Assert
    // }

    [Fact]
    public async Task ExecProcedureAsync_TruncateUsers_RetursSuccess()
    {
        //Arrange
        var parameters = new { Option = "Truncate" };

        //Act
        var exception = await Record.ExceptionAsync(async () =>
            await _procedureService.ExecProcedureAsync(parameters));

        //Assert
        Assert.Null(exception);
    }

    [Theory]
    [MemberData(nameof(TestUsersData))]
    public async Task ExecScalarProcedureAsync_CreateUser_RetursSuccessGuid(User user)
    {
        //Arrange
        var options = new { Option = "Create" };
        
        //Act
        var result = await _procedureService.ExecScalarProcedureAsync<Guid>(options, user);

        //Assert
        Assert.IsType<Guid>(result);
        _testOutputHelper.WriteLine(result.ToString());
    }

    public static IEnumerable<object[]> TestUsersData()
    {
        var users = new UsersTestData().GetUsers(5);
        foreach (var user in users)
        {
            yield return new object[] { user };
        }
    }
}