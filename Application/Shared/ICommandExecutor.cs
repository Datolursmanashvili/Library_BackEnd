using Shared;

namespace Application.Shared;

public interface ICommandExecutor
{
    Task<RepositoryExecutionResult> Execute(Command command);
    Task<CommandExecutionResultGeneric<T>> Execute<T>(Command<T> command);
}