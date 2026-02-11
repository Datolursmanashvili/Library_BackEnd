using Application.Commands.UserCommands;
using Application.Queries.UserQuery;
using Application.Shared;
using Microsoft.AspNetCore.Mvc;
using Shared;
using static Application.Queries.UserQuery.LoginQuery;

namespace Interface.Controllers;

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



    //[Route("ForgetPassword")]
    //[HttpPost]
    //public async Task<CommandExecutionResultGeneric<ForgetPasswordAspNetUserCommandResult>> ResetPassword([FromBody] ForgetPasswordAspNetUserCommand command) =>
    //     await _commandExecutor.Execute(command);

    //[Route("CahngePasswrodByAdmin")]
    //[HttpPost]
    //public async Task<CommandExecutionResultGeneric<ChangePasswordByAdminAspNetUserCommandResult>> CahngePasswrodByAdmin([FromBody] ChangePasswordByAdminAspNetUserCommand command) =>
    //await _commandExecutor.Execute(command);

    //[Route("CahngePasswrodForAdmin")]
    //[HttpPost]
    //public async Task<CommandExecutionResultGeneric<CahngePasswrodForAdminCommandResult>> CahngePasswrodForAdmin([FromBody] CahngePasswrodForAdminCommand command) =>
    //  await _commandExecutor.Execute(command);

    //[Route("ResetPasswordByAdmin")]
    //[HttpPost]
    //public async Task<CommandExecutionResultGeneric<ResetPasswordByAdminResult>> ResetPasswordByAdmin([FromBody] ResetPasswordByAdmin command) =>
    //      await _commandExecutor.Execute(command);

    //[Route("ResetPassword")]
    //[HttpPost]
    //public async Task<CommandExecutionResultGeneric<ResetPasswordCommandResult>> ResetPassword([FromBody] ResetPasswordCommand command) =>
    //    await _commandExecutor.Execute(command);


    //[Route("SetUserDefaultPassword")]
    //[HttpPost]
    //public async Task<CommandExecutionResultGeneric<SetUserDefaultPasswordCommandResult>> SetUserDefaultPassword([FromBody] SetUserDefaultPasswordCommand command) =>
    //    await _commandExecutor.Execute(command);

    //[Route("activatenewuser")]
    //[HttpPost]
    //public async Task<CommandExecutionResultGeneric<ActivateNewUserCommandResult>> activatenewuser([FromBody] ActivateNewUserCommand command) =>
    //      await _commandExecutor.Execute(command);

    #endregion
}
