namespace Nebula.Infrastructure.Commanding
{
    public interface ICommandHandlerFactory
    {
        ICommandHandler<TCommand> CreateHandler<TCommand>()
            where TCommand : ICommand;

        ICommandHandler<TCommand, TResult> CreateHandler<TCommand, TResult>()
            where TCommand : ICommand
            where TResult : ICommandResult;

        void ReleaseHandler(ICommandHandler commandHandler);
    }
}