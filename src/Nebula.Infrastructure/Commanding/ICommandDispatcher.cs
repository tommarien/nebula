namespace Nebula.Infrastructure.Commanding
{
    public interface ICommandDispatcher
    {
        void Dispatch<TCommand>(TCommand command)
            where TCommand : ICommand;

        TResult Dispatch<TCommand, TResult>(TCommand command)
            where TCommand : ICommand
            where TResult : ICommandResult;
    }
}