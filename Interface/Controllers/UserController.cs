using Application.Commands.UserCommands;
using Application.Queries.UserQuery;
using Application.Shared;
using Interface.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using static Application.Queries.UserQuery.LoginQuery;

namespace Interface.Controllers;

/// <summary>
/// User authentication, registration, and user listing.
/// </summary>
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ICommandExecutor _commandExecutor;
    private readonly IQueryExecutor _queryExecutor;

    /// <summary>Creates the controller.</summary>
    public UserController(
        ICommandExecutor commandExecutor,
        IQueryExecutor queryExecutor)
    {
        _commandExecutor = commandExecutor;
        _queryExecutor = queryExecutor;
    }

    #region Queries

    /// <summary>
    /// Authenticates a user by email and password and returns a JWT access token.
    /// </summary>
    /// <param name="query">Email and password (query string).</param>
    /// <returns>Wrapped <see cref="LoginQueryResult"/> on success; error details in the envelope on failure (400/401/403/404).</returns>
    [AllowAnonymous]
    [Route("Login")]
    [HttpGet]
    [ProducesResponseType(typeof(QueryExecutionResult<LoginQueryResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(QueryExecutionResult<LoginQueryResult>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(QueryExecutionResult<LoginQueryResult>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(QueryExecutionResult<LoginQueryResult>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(QueryExecutionResult<LoginQueryResult>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Login([FromQuery] LoginQuery query)
    {
        var result = await _queryExecutor.Execute<LoginQuery, LoginQueryResult>(query);
        return this.ToActionResult(result);
    }

    /// <summary>
    /// Returns a paginated list of users (1-based page index).
    /// </summary>
    /// <param name="query">Paging: 1-based <c>Page</c> and <c>PageSize</c> (see Application.Shared.PagedQuery).</param>
    /// <returns>Wrapped <see cref="GetAllUserQueryResult"/>.</returns>
    [AllowAnonymous]
    [Route("GetAllUser")]
    [HttpGet]
    [ProducesResponseType(typeof(QueryExecutionResult<GetAllUserQueryResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(QueryExecutionResult<GetAllUserQueryResult>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllUser([FromQuery] GetAllUserQuery query)
    {
        var result = await _queryExecutor.Execute<GetAllUserQuery, GetAllUserQueryResult>(query);
        return this.ToActionResult(result);
    }
    #endregion


    #region Commands

    /// <summary>
    /// Registers a new user account (default role: user). Returns 201 Created on success.
    /// </summary>
    /// <param name="command">Registration fields (JSON body).</param>
    /// <returns>Wrapped <see cref="UserCommandResult"/>; 409 if email/username/PNumber already exists.</returns>
    [AllowAnonymous]
    [Route("Registration")]
    [HttpPost]
    [ProducesResponseType(typeof(CommandExecutionResultGeneric<UserCommandResult>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(CommandExecutionResultGeneric<UserCommandResult>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CommandExecutionResultGeneric<UserCommandResult>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Registration([FromBody] RegistrationCommand command)
    {
        var result = await _commandExecutor.Execute(command);
        return this.ToActionResult(result);
    }

    #endregion
}
