using Application.Commands.AuthorCommands;
using Application.Queries.AuthorQueries;
using Application.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Interface.Controllers
{
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

        [Route("GetAllAuthors")]
        [HttpGet]
        [Authorize(Roles = UserGroups.Admin)]
        public async Task<QueryExecutionResult<GetAllAuthorsQueryResult>> GetAllAuthors([FromQuery] GetAllAuthorsQuery query) =>
            await _queryExecutor.Execute<GetAllAuthorsQuery, GetAllAuthorsQueryResult>(query);

        [Route("GetAuthorById")]
        [HttpGet]
        [Authorize(Roles = UserGroups.Admin)]
        public async Task<QueryExecutionResult<AuthorQueryResultItem>> GetAuthorById([FromQuery] GetAuthorByIdQuery query) =>
            await _queryExecutor.Execute<GetAuthorByIdQuery, AuthorQueryResultItem>(query);

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
        [Route("DeleteAuthor/{id}")]
        [HttpDelete]
        [Authorize(Roles = UserGroups.Admin)]
        public async Task<CommandExecutionResult> DeleteAuthor([FromRoute] int id)
        {
            var command = new DeleteAuthorCommand { Id = id };
            var result = await _commandExecutor.Execute(command);

            if (result.Success)
            {
                return new CommandExecutionResult
                {
                    Success = true
                };
            }
            else
            {
                return new CommandExecutionResult
                {
                    Success = false,
                    ErrorMessage = "წაშლა ვერ განხორციელდა"
                };
            }
        }


        #endregion
    }
}
