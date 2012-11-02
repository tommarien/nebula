namespace Nebula.Infrastructure.Commanding
{
    public interface ICommandHandler
    {
    }

    public interface ICommandHandler<TCommand> : IHandle<TCommand>, ICommandHandler
        where TCommand : ICommand
    {
    }

    public interface ICommandHandler<TCommand, TResult> : IHandleAndReply<TCommand, TResult>, ICommandHandler
        where TCommand : ICommand
        where TResult : ICommandResult
    {
    }
}