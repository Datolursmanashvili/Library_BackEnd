using Application.Commands.LocationCommands;
using Application.Queries.LocationQueries;
using Application.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Interface.Controllers;

[ApiController]
[Route("[controller]")]
public class LocationController : ControllerBase
{
    private readonly ICommandExecutor _commandExecutor;
    private readonly IQueryExecutor _queryExecutor;

    public LocationController(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor)
    {
        _commandExecutor = commandExecutor;
        _queryExecutor = queryExecutor;
    }

    #region Queries

    [Route("GetAllCountry")]
    [HttpGet]
    [Authorize(Roles = UserGroups.Admin)]
    public async Task<QueryExecutionResult<List<LocationResult>?>> GetAllCountry([FromQuery] GetAllCountryQuery query) =>
        await _queryExecutor.Execute<GetAllCountryQuery, List<LocationResult>?>(query);

    [Route("GetCountryById")]
    [HttpGet]
    [Authorize(Roles = UserGroups.Admin)]
    public async Task<QueryExecutionResult<LocationResult?>> GetCountryById([FromQuery] GetCountryByIdQuery query) =>
        await _queryExecutor.Execute<GetCountryByIdQuery, LocationResult?>(query);

    [Route("GetAllCityByCountryId")]
    [HttpGet]
    [Authorize(Roles = UserGroups.Admin)]
    public async Task<QueryExecutionResult<List<LocationResult>?>> GetAllCityByCountryId([FromQuery] GetAllCityByCountryIdQuery query) =>
        await _queryExecutor.Execute<GetAllCityByCountryIdQuery, List<LocationResult>?>(query);

    [Route("GetCountryAndCityList")]
    [HttpGet]
    [Authorize(Roles = UserGroups.Admin)]
    public async Task<QueryExecutionResult<List<CountryWithCitiesResult>>> GetCountryAndCityList([FromQuery] GetCountryAndCityListQuery query) =>
        await _queryExecutor.Execute<GetCountryAndCityListQuery, List<CountryWithCitiesResult>>(query);

    #endregion

    #region Commands

    [Route("AddLocation")]
    [HttpPost]
    [Authorize(Roles = UserGroups.Admin)]
    public async Task<CommandExecutionResultGeneric<LocationResult>> AddLocation([FromBody] AddLocationCommand command) =>
        await _commandExecutor.Execute(command);

    [Route("UpdateLocation")]
    [HttpPut]
    [Authorize(Roles = UserGroups.Admin)]
    public async Task<CommandExecutionResultGeneric<LocationResult>> UpdateLocation([FromBody] UpdateLocationCommand command) =>
        await _commandExecutor.Execute(command);

    [Route("DeleteLocation/{id}")]
    [HttpDelete]
    [Authorize(Roles = UserGroups.Admin)]
    public async Task<CommandExecutionResult> DeleteLocation([FromRoute] int id)
    {
        var command = new DeleteLocationCommand { Id = id };
        var result = await _commandExecutor.Execute(command);

        return result.Success
            ? new CommandExecutionResult { Success = true }
            : new CommandExecutionResult { Success = false, ErrorMessage = "წაშლა ვერ განხორციელდა" };
    }

    #endregion
}
