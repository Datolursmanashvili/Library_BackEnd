using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Interface.Extensions;

public static class ApiResultExtensions
{
    public static IActionResult ToActionResult<T>(this ControllerBase _, QueryExecutionResult<T> result)
    {
        var status = ResolveStatusCode(result.Success, result.HttpStatusCode, result.Errors);
        return new ObjectResult(result) { StatusCode = status };
    }

    public static IActionResult ToActionResult<T>(this ControllerBase _, CommandExecutionResultGeneric<T> result)
    {
        var status = ResolveStatusCode(result.Success, result.HttpStatusCode, result.Errors);
        return new ObjectResult(result) { StatusCode = status };
    }

    public static IActionResult ToActionResult(this ControllerBase _, RepositoryExecutionResult result)
    {
        if (result.Success)
            return new ObjectResult(result) { StatusCode = StatusCodes.Status200OK };
        var status = result.Code is >= 400 and < 600
            ? result.Code
            : StatusCodes.Status400BadRequest;
        return new ObjectResult(result) { StatusCode = status };
    }

    private static int ResolveStatusCode(bool success, int httpStatusCode, IEnumerable<Error>? errors)
    {
        if (success)
        {
            if (httpStatusCode is >= 200 and < 300)
                return httpStatusCode;
            return StatusCodes.Status200OK;
        }

        if (httpStatusCode is >= 400 and < 600)
            return httpStatusCode;

        var fromError = errors?.FirstOrDefault(e => e.Code >= 400 && e.Code < 600)?.Code;
        return fromError ?? StatusCodes.Status400BadRequest;
    }
}
