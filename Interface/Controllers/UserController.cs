using Application.Commands.UserCommands;
using Application.Queries.UserQuery;
using Application.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using static Application.Queries.UserQuery.LoginQuery;

namespace Interface.Controllers;

//[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ICommandExecutor _commandExecutor;
    private readonly IQueryExecutor _queryExecutor;

    public UserController(
        ICommandExecutor commandExecutor,
        IQueryExecutor queryExecutor)
    {
        _commandExecutor = commandExecutor;
        _queryExecutor = queryExecutor;
    }

    #region Queries

    [Route("Login")]
    [HttpGet]
    public async Task<QueryExecutionResult<LoginQueryResult>> Login([FromQuery] LoginQuery query) =>
     await _queryExecutor.Execute<LoginQuery, LoginQueryResult>(query);

    [Authorize(Roles = UserGroups.All)]
    [Route("GetAllUser")]
    [HttpGet]
    public async Task<QueryExecutionResult<GetAllUserQueryResult>> GetAllUser([FromQuery] GetAllUserQuery query) =>
     await _queryExecutor.Execute<GetAllUserQuery, GetAllUserQueryResult>(query);
    #endregion


    #region Commands
    [Route("Registration")]
    [HttpPost]
    public async Task<CommandExecutionResultGeneric<UserCommandResult>> Registration([FromBody] RegistrationCommand command) =>
         await _commandExecutor.Execute(command);


    #endregion
}
