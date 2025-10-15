namespace Shared;

public class ResponseHelper
{
    // Original methods for non-generic CommandExecutionResult
    protected Task<CommandExecutionResult> Fail(string? errorMessage)
    {
        var result = new CommandExecutionResult
        {
            Success = false
        };
        result.Errors = new List<Error>
        {
            new Error
            {
                Message = errorMessage ?? string.Empty
            }
        };
        return Task.FromResult(result);
    }

    protected Task<CommandExecutionResult> Ok(string resultId, long? listCount = null)
    {
        var result = new CommandExecutionResult
        {
            ResultId = resultId,
            Success = true,
            ListCount = listCount,
            Code = 200
        };
        return Task.FromResult(result);
    }

    // New generic methods for CommandExecutionResultGeneric<T>
    protected Task<CommandExecutionResultGeneric<T>> Fail<T>(string? errorMessage)
    {
        var result = new CommandExecutionResultGeneric<T>
        {
            Success = false,
            Errors = new List<Error>
            {
                new Error
                {
                    Message = errorMessage ?? string.Empty
                }
            }
        };
        return Task.FromResult(result);
    }

    protected Task<CommandExecutionResultGeneric<T>> Ok<T>(T data)
    {
        var result = new CommandExecutionResultGeneric<T>
        {
            Success = true,
            Data = data
        };
        return Task.FromResult(result);
    }
}