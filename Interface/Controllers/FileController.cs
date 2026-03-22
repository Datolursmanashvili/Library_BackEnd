using Application.Commands.FileCommands;
using Application.Shared;
using Interface.Extensions;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Interface.Controllers;

/// <summary>
/// File upload helpers (base64 payload).
/// </summary>
[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
    private readonly ICommandExecutor _commandExecutor;
    private readonly IQueryExecutor _queryExecutor;

    /// <summary>Creates the controller.</summary>
    public FileController(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor)
    {
        _commandExecutor = commandExecutor;
        _queryExecutor = queryExecutor;
    }

    /// <summary>
    /// Saves a file from a base64 string to server storage and returns a public URL fragment/path.
    /// </summary>
    /// <param name="command">File name, extension, and base64 content (JSON body).</param>
    /// <returns>Wrapped <see cref="FileCommandResult"/> with file path/URL; 400 if validation or format fails.</returns>
    [Route("SaveFile")]
    [HttpPost]
    [ProducesResponseType(typeof(CommandExecutionResultGeneric<FileCommandResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CommandExecutionResultGeneric<FileCommandResult>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddAuthor([FromBody] SaveFileCommand command)
    {
        var result = await _commandExecutor.Execute(command);
        return this.ToActionResult(result);
    }
}
