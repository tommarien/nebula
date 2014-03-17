namespace Nebula.Infrastructure.Commanding
{
    public interface ICommandHandler
    {
    }

    public interface ICommandHandler<in TCommand> : IHandle<TCommand>, ICommandHandler
        where TCommand : ICommand
    {
    }

    public interface ICommandHandler<in TCommand, out TResult> : IHandleAndReply<TCommand, TResult>, ICommandHandler
        where TCommand : ICommand
    {
    }
}