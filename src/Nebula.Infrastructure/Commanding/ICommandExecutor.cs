namespace Nebula.Infrastructure.Commanding
{
    public interface ICommandExecutor
    {
        void Execute<TCommand>(TCommand command) 
            where TCommand : ICommand;

        TResult Execute<TCommand, TResult>(TCommand command)
            where TCommand : ICommand
            where TResult : ICommandResult;
    }
}