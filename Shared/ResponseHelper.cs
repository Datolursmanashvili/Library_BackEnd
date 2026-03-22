namespace Shared;

public class ResponseHelper
{
    protected Task<RepositoryExecutionResult> Fail(string? errorMessage) =>
        Fail(400, errorMessage);

    protected Task<RepositoryExecutionResult> Fail(int httpStatusCode, string? errorMessage)
    {
        var result = new RepositoryExecutionResult
        {
            Success = false,
            Code = httpStatusCode,
            ErrorMessage = errorMessage,
            Errors = new List<Error>
            {
                new Error
                {
                    Message = errorMessage ?? string.Empty,
                    Code = httpStatusCode
                }
            }
        };
        return Task.FromResult(result);
    }

    protected Task<RepositoryExecutionResult> Ok(string resultId, long? listCount = null)
    {
        var result = new RepositoryExecutionResult
        {
            ResultId = resultId,
            Success = true,
            ListCount = listCount,
            Code = 200
        };
        return Task.FromResult(result);
    }

    protected Task<CommandExecutionResultGeneric<T>> Fail<T>(string? errorMessage) =>
        Fail<T>(400, errorMessage);

    protected Task<CommandExecutionResultGeneric<T>> Fail<T>(int httpStatusCode, string? errorMessage)
    {
        var result = new CommandExecutionResultGeneric<T>
        {
            Success = false,
            HttpStatusCode = httpStatusCode,
            Errors = new List<Error>
            {
                new Error
                {
                    Message = errorMessage ?? string.Empty,
                    Code = httpStatusCode
                }
            }
        };
        return Task.FromResult(result);
    }

    protected Task<CommandExecutionResultGeneric<T>> Ok<T>(T data) =>
        Ok(data, 200);

    protected Task<CommandExecutionResultGeneric<T>> Ok<T>(T data, int httpStatusCode)
    {
        var result = new CommandExecutionResultGeneric<T>
        {
            Success = true,
            Data = data,
            HttpStatusCode = httpStatusCode
        };
        return Task.FromResult(result);
    }
}