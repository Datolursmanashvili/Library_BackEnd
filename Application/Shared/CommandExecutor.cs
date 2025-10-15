using FluentValidation;
using FluentValidation.Results;
using Infrastructure.DB;
using Microsoft.Extensions.Configuration;
using Shared;
using System.Reflection;

namespace Application.Shared;

public class CommandExecutor : ICommandExecutor
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;

    public CommandExecutor(
        IServiceProvider serviceProvider,
        ApplicationDbContext applicationDbContext,
        IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _applicationDbContext = applicationDbContext;
        _configuration = configuration;
    }

    public async Task<CommandExecutionResult> Execute(Command command)
    {
        try
        {
            var validationResult = Validate(command);
            if (!validationResult.IsValid)
            {
                return new CommandExecutionResult
                {
                    Success = false,
                    Errors = validationResult.Errors.Select(error => new Error { Message = error.ErrorMessage, Code = 0 })
                };
            }

            command.Resolve(_applicationDbContext, _serviceProvider, _configuration);

            var commandResult = await command.ExecuteAsync();

            return commandResult;
        }
        catch (Exception ex)
        {
            return new CommandExecutionResult
            {
                Success = false,
                Errors = new List<Error>
                {
                    new Error
                    {
                        Code = 0,
                        Message = ex.ToString() // TEMP:
                    }
                }
            };
        }
    }

    public async Task<CommandExecutionResultGeneric<T>> Execute<T>(Command<T> command)
    {
        try
        {
            var validationResult = Validate(command);
            if (!validationResult.IsValid)
            {
                return new CommandExecutionResultGeneric<T>
                {
                    Success = false,
                    Errors = validationResult.Errors.Select(error => new Error { Message = error.ErrorMessage, Code = 0 })
                };
            }

            command.Resolve(_applicationDbContext, _serviceProvider, _configuration);

            var commandResult = await command.ExecuteAsync();

            return commandResult;
        }
        catch (Exception ex)
        {
            return new CommandExecutionResultGeneric<T>
            {
                Success = false,
                Errors = new List<Error>
                {
                    new Error
                    {
                        Code = 0,
                        Message = ex.ToString() // TEMP:
                    }
                }
            };
        }
    }

    public ValidationResult Validate<T>(T command)
    {
        var validatorType = typeof(IValidator<>).MakeGenericType(command.GetType());
        var validator = (IValidator?)_serviceProvider.GetService(validatorType);

        if (validator != null)
        {
            var modelState = validator.Validate(new ValidationContext<object>(command));
            return modelState;
        }

        return new ValidationResult();
    }
}