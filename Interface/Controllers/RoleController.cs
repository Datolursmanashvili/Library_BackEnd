using Application.Commands.RoleCommands;
using Application.Queries.RoleQueries;
using Application.Shared;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Interface.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly ICommandExecutor _commandExecutor;
        private readonly IQueryExecutor _queryExecutor;

        public RoleController(
            ICommandExecutor commandExecutor,
            IQueryExecutor queryExecutor)
        {
            _commandExecutor = commandExecutor;
            _queryExecutor = queryExecutor;
        }

        #region commands
        [Route("AddNewRole")]
        [HttpPost]
        public async Task<RepositoryExecutionResult> AddNewRole([FromBody] AddNewRoleCommand command) =>
         await _commandExecutor.Execute(command);

        [Route("EditRoleName")]
        [HttpPut]
        public async Task<RepositoryExecutionResult> EditRoleName([FromBody] EditRoleNameCommand command) =>
          await _commandExecutor.Execute(command);
        #endregion

        #region queries

        [Route("GetRoleById")]
        [HttpPost]
        public async Task<QueryExecutionResult<GetRoleByIdQueryResult>> GetRoleById([FromBody] GetRoleByIdQuery query) =>
            await _queryExecutor.Execute<GetRoleByIdQuery, GetRoleByIdQueryResult>(query);

        [Route("GetAllRoles")]
        [HttpGet]
        public async Task<QueryExecutionResult<List<RoleItemResponseItem>?>> GetAllRoles([FromQuery] GetAllRoles query) =>
            await _queryExecutor.Execute<GetAllRoles, List<RoleItemResponseItem>?>(query);

        #endregion


        //[Route("EditRolePermissions")]
        //[HttpPut]
        //public async Task<CommandExecutionResult> EditRolePermissions([FromBody] EditRolePermissionsCommand command) =>
        //  await _commandExecutor.Execute(command);



        //[Route("DeleteRole")]
        //[HttpDelete]
        //public async Task<CommandExecutionResult> DeleteRole([FromBody] DeleteRoleCommand command) =>
        // await _commandExecutor.Execute(command);

        //[Route("GetRoles")]
        //[HttpGet]
        //public async Task<QueryExecutionResult<GetRolesQueryResult>> GetRoles([FromQuery] GetRolesQuery query) =>
        //     await _queryExecutor.Execute<GetRolesQuery, GetRolesQueryResult>(query);

        //[Route("GetRoleForFilters")]
        //[HttpGet]
        //public async Task<QueryExecutionResult<GetRoleForFiltersQueryResult>> GetRoleForFilters([FromQuery] GetRoleForFiltersQuery query) =>
        //     await _queryExecutor.Execute<GetRoleForFiltersQuery, GetRoleForFiltersQueryResult>(query);
    }
}
