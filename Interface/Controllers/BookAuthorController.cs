using Application.Commands.BookAuthorCommands;
using Application.Queries.BookAuthorQueries;
using Application.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Interface.Controllers;

[ApiController]
[Route("[controller]")]
public class BookAuthorController : ControllerBase
{
    private readonly ICommandExecutor _commandExecutor;
    private readonly IQueryExecutor _queryExecutor;

    public BookAuthorController(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor)
    {
        _commandExecutor = commandExecutor;
        _queryExecutor = queryExecutor;
    }

    #region Queries

    [Route("GetAll")]
    [HttpGet]
    [Authorize(Roles = UserGroups.Admin)]
    public async Task<QueryExecutionResult<List<GetAllBookAuthorsQuery.BookAuthorQueryResult>>> GetAll() =>
        await _queryExecutor.Execute<GetAllBookAuthorsQuery, List<GetAllBookAuthorsQuery.BookAuthorQueryResult>>(new GetAllBookAuthorsQuery());

    #endregion

    #region Commands

    [Route("Add")]
    [HttpPost]
    [Authorize(Roles = UserGroups.Admin)]
    public async Task<CommandExecutionResultGeneric<AddBookAuthorCommand.BookAuthorCommandResult>> Add([FromBody] AddBookAuthorCommand command) =>
        await _commandExecutor.Execute(command);

    [Route("Delete/{id}")]
    [HttpDelete]
    [Authorize(Roles = UserGroups.Admin)]
    public async Task<CommandExecutionResult> Delete([FromRoute] int id)
    {
        var result = await _commandExecutor.Execute(new DeleteBookAuthorCommand { Id = id });
        if (result.Success)
            return new CommandExecutionResult { Success = true };

        return new CommandExecutionResult { Success = false, ErrorMessage = "წაშლა ვერ განხორციელდა" };
    }

    #endregion
}
