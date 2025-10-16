using Application.Commands.AuthorCommands;
using Application.Queries.AuthorQueries;
using Application.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Interface.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorController : ControllerBase
{
    private readonly ICommandExecutor _commandExecutor;
    private readonly IQueryExecutor _queryExecutor;

    public AuthorController(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor)
    {
        _commandExecutor = commandExecutor;
        _queryExecutor = queryExecutor;
    }

    #region Queries

    [Route("GetAllAuthorsByFiltres")]
    [HttpGet]
    [Authorize(Roles = UserGroups.Admin)]
    public async Task<QueryExecutionResult<GetAllAuthorsQueryResult>> GetAllAuthorsByFiltres([FromQuery] GetAllAuthorsByFiltresQuery query) =>
        await _queryExecutor.Execute<GetAllAuthorsByFiltresQuery, GetAllAuthorsQueryResult>(query);

    [Route("GetAuthorProductInfoByAuthorId")]
    [HttpGet]
    [Authorize(Roles = UserGroups.Admin)]
    public async Task<QueryExecutionResult<AuthorProductInfoResult>> GetAuthorProductInfoByAuthorId([FromQuery] GetAuthorProductInfoByAuthorIdQuery query) =>
        await _queryExecutor.Execute<GetAuthorProductInfoByAuthorIdQuery, AuthorProductInfoResult>(query);

    #endregion

    #region Commands

    [Route("AddAuthor")]
    [HttpPost]
    [Authorize(Roles = UserGroups.Admin)]
    public async Task<CommandExecutionResultGeneric<AuthorCommandResult>> AddAuthor([FromBody] AddAuthorCommand command) =>
        await _commandExecutor.Execute(command);

    [Route("UpdateAuthor")]
    [HttpPut]
    [Authorize(Roles = UserGroups.Admin)]
    public async Task<CommandExecutionResultGeneric<AuthorCommandResult>> UpdateAuthor([FromBody] UpdateAuthorCommand command) =>
        await _commandExecutor.Execute(command);

    #region author Delete
    //[Route("DeleteAuthor/{id}")]
    //[HttpDelete]
    //[Authorize(Roles = UserGroups.Admin)]
    //public async Task<CommandExecutionResult> DeleteAuthor([FromRoute] int id)
    //{
    //    var command = new DeleteAuthorCommand { Id = id };
    //    var result = await _commandExecutor.Execute(command);

    //    if (result.Success)
    //    {
    //        return new CommandExecutionResult
    //        {
    //            Success = true
    //        };
    //    }
    //    else
    //    {
    //        return new CommandExecutionResult
    //        {
    //            Success = false,
    //            ErrorMessage = "წაშლა ვერ განხორციელდა"
    //        };
    //    }
    //}
    #endregion

    #endregion
}
