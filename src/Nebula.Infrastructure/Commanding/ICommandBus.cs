namespace Nebula.Infrastructure.Commanding
{
    public interface ICommandBus
    {
        void Send<TCommand>(TCommand command)
            where TCommand : ICommand;

        TResult SendAndReply<TCommand, TResult>(TCommand command)
            where TCommand : ICommand
            where TResult : ICommandResult;
    }
}