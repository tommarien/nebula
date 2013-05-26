namespace Nebula.Infrastructure.Commanding
{
    public interface ICommandHandlerFactory
    {
        ICommandHandler<TCommand> CreateHandler<TCommand>()
            where TCommand : ICommand;

        void ReleaseHandler(ICommandHandler commandHandler);
    }
}