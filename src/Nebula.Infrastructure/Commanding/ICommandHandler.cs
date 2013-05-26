namespace Nebula.Infrastructure.Commanding
{
    public interface ICommandHandler
    {
    }

    public interface ICommandHandler<TCommand> : IHandle<TCommand>, ICommandHandler
        where TCommand : ICommand
    {
    }
}