using Application.Commands.PublisherCommands;
using Application.Queries.PublisherQueries;
using Application.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Interface.Controllers;

[ApiController]
[Route("[controller]")]
public class PublisherController : ControllerBase
{
    private readonly ICommandExecutor _commandExecutor;
    private readonly IQueryExecutor _queryExecutor;

    public PublisherController(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor)
    {
        _commandExecutor = commandExecutor;
        _queryExecutor = queryExecutor;
    }

    #region Queries

    [HttpGet("GetAllPublishers")]
    [Authorize(Roles = UserGroups.Admin)]
    public async Task<QueryExecutionResult<List<PublisherCommandResult>>> GetAllPublishers([FromQuery] GetAllPublishersQuery query) =>
        await _queryExecutor.Execute<GetAllPublishersQuery, List<PublisherCommandResult>>(query);

    [HttpGet("GetPublisherById")]
    [Authorize(Roles = UserGroups.Admin)]
    public async Task<QueryExecutionResult<PublisherCommandResult>> GetPublisherById([FromQuery] GetPublisherByIdQuery query) =>
        await _queryExecutor.Execute<GetPublisherByIdQuery, PublisherCommandResult>(query);

    #endregion

    #region Commands

    [HttpPost("AddPublisher")]
    [Authorize(Roles = UserGroups.Admin)]
    public async Task<CommandExecutionResultGeneric<PublisherCommandResult>> AddPublisher([FromBody] AddPublisherCommand command) =>
        await _commandExecutor.Execute(command);

    [HttpPut("UpdatePublisher")]
    [Authorize(Roles = UserGroups.Admin)]
    public async Task<CommandExecutionResultGeneric<PublisherCommandResult>> UpdatePublisher([FromBody] UpdatePublisherCommand command) =>
        await _commandExecutor.Execute(command);

    [HttpDelete("DeletePublisher/{id}")]
    [Authorize(Roles = UserGroups.Admin)]
    public async Task<CommandExecutionResult> DeletePublisher([FromRoute] int id)
    {
        var command = new DeletePublisherCommand { Id = id };
        var result = await _commandExecutor.Execute(command);

        return result.Success
            ? new CommandExecutionResult { Success = true }
            : new CommandExecutionResult { Success = false, ErrorMessage =  "Deletion failed" };
    }

    #endregion
}
