using Application.Commands.RoleCommands;
using Application.Queries.RoleQueries;
using Application.Shared;
using Interface.Extensions;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Interface.Controllers
{
    /// <summary>
    /// Role management and role queries.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly ICommandExecutor _commandExecutor;
        private readonly IQueryExecutor _queryExecutor;

        /// <summary>Creates the controller.</summary>
        public RoleController(
            ICommandExecutor commandExecutor,
            IQueryExecutor queryExecutor)
        {
            _commandExecutor = commandExecutor;
            _queryExecutor = queryExecutor;
        }

        #region commands

        /// <summary>
        /// Creates a new role with the given name and permissions.
        /// </summary>
        /// <param name="command">Role name and permission set (JSON body).</param>
        /// <returns>Operation result; 409 if the role name already exists.</returns>
        [Route("AddNewRole")]
        [HttpPost]
        [ProducesResponseType(typeof(RepositoryExecutionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RepositoryExecutionResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RepositoryExecutionResult), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(RepositoryExecutionResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddNewRole([FromBody] AddNewRoleCommand command)
        {
            var result = await _commandExecutor.Execute(command);
            return this.ToActionResult(result);
        }

        /// <summary>
        /// Renames an existing role.
        /// </summary>
        /// <param name="command">Role id and new name (JSON body).</param>
        /// <returns>Operation result; 404 if role missing; 409 if name conflicts.</returns>
        [Route("EditRoleName")]
        [HttpPut]
        [ProducesResponseType(typeof(RepositoryExecutionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RepositoryExecutionResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RepositoryExecutionResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RepositoryExecutionResult), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(RepositoryExecutionResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditRoleName([FromBody] EditRoleNameCommand command)
        {
            var result = await _commandExecutor.Execute(command);
            return this.ToActionResult(result);
        }
        #endregion

        #region queries

        /// <summary>
        /// Returns a single role by identifier (including permissions).
        /// </summary>
        /// <param name="query">Role id (JSON body).</param>
        /// <returns>Wrapped <see cref="GetRoleByIdQueryResult"/>; 404 if not found.</returns>
        [Route("GetRoleById")]
        [HttpPost]
        [ProducesResponseType(typeof(QueryExecutionResult<GetRoleByIdQueryResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(QueryExecutionResult<GetRoleByIdQueryResult>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(QueryExecutionResult<GetRoleByIdQueryResult>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRoleById([FromBody] GetRoleByIdQuery query)
        {
            var result = await _queryExecutor.Execute<GetRoleByIdQuery, GetRoleByIdQueryResult>(query);
            return this.ToActionResult(result);
        }

        /// <summary>
        /// Returns a paginated list of non-deleted roles.
        /// </summary>
        /// <param name="query">Paging parameters (query string).</param>
        /// <returns>Wrapped <see cref="GetAllRolesQueryResult"/>.</returns>
        [Route("GetAllRoles")]
        [HttpGet]
        [ProducesResponseType(typeof(QueryExecutionResult<GetAllRolesQueryResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(QueryExecutionResult<GetAllRolesQueryResult>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllRoles([FromQuery] GetAllRolesQuery query)
        {
            var result = await _queryExecutor.Execute<GetAllRolesQuery, GetAllRolesQueryResult>(query);
            return this.ToActionResult(result);
        }

        #endregion
    }
}
