using Application.Commands.AuthorCommands;
using Application.Commands.FileCommands;
using Application.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Interface.Controllers;

public class FileController : ControllerBase
{
    private readonly ICommandExecutor _commandExecutor;
    private readonly IQueryExecutor _queryExecutor;

    public FileController(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor)
    {
        _commandExecutor = commandExecutor;
        _queryExecutor = queryExecutor;
    }

    [Route("SaveFile")]
    [HttpPost]
    //[Authorize(Roles = UserGroups.Admin)]
    public async Task<CommandExecutionResultGeneric<FileCommandResult>> AddAuthor([FromBody] SaveFileCommand command) =>
       await _commandExecutor.Execute(command);

}