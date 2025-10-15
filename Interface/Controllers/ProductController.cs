using Application.Commands.ProductsCommands;
using Application.Queries.ProductQueries;
using Application.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Interface.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ICommandExecutor _commandExecutor;
        private readonly IQueryExecutor _queryExecutor;

        public ProductController(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor)
        {
            _commandExecutor = commandExecutor;
            _queryExecutor = queryExecutor;
        }

        #region Queries

        [Route("GetAllProducts")]
        [HttpGet]
        [Authorize(Roles = UserGroups.All)]
        public async Task<QueryExecutionResult<GetAllProductsQueryResult>> GetAllProducts([FromQuery] GetAllProductsQuery query) =>
            await _queryExecutor.Execute<GetAllProductsQuery, GetAllProductsQueryResult>(query);

        [Route("GetProductById/{id}")]
        [HttpGet]
        [Authorize(Roles = UserGroups.All)]
        public async Task<QueryExecutionResult<ProductQueryResultItem>> GetProductById([FromRoute] int id) =>
            await _queryExecutor.Execute<GetProductByIdQuery, ProductQueryResultItem>(new GetProductByIdQuery { Id = id });

        #endregion

        #region Commands

        [Route("AddProduct")]
        [HttpPost]
        [Authorize(Roles = UserGroups.All)]
        public async Task<CommandExecutionResultGeneric<ProductCommandResult>> AddProduct([FromBody] AddProductCommand command) =>
            await _commandExecutor.Execute(command);

        [Route("UpdateProduct")]
        [HttpPut]
        [Authorize(Roles = UserGroups.All)]
        public async Task<CommandExecutionResultGeneric<ProductCommandResult>> UpdateProduct([FromBody] UpdateProductCommand command) =>
            await _commandExecutor.Execute(command);

        [Route("DeleteProduct/{id}")]
        [HttpDelete]
        [Authorize(Roles = UserGroups.Admin)]
        public async Task<CommandExecutionResult> DeleteProduct([FromRoute] int id)
        {
            var command = new DeleteProductCommand { Id = id };
            var result = await _commandExecutor.Execute(command);

            if (result.Success)
            {
                return new CommandExecutionResult { Success = true };
            }
            else
            {
                return new CommandExecutionResult
                {
                    Success = false,
                    ErrorMessage = "პროდუქტის წაშლა ვერ განხორციელდა"
                };
            }
        }

        #endregion
    }
}
