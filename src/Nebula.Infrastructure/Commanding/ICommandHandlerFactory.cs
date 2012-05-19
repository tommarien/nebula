namespace Nebula.Infrastructure.Commanding
{
    public interface ICommandHandlerFactory
    {
        ICommandHandler<TCommand> CreateHandler<TCommand>() where TCommand : ICommand;
        void ReleaseHandler<TCommand>(ICommandHandler<TCommand> commandHandler) where TCommand : ICommand;
    }
}