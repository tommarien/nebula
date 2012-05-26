namespace Nebula.Infrastructure.Commanding
{
    public interface ICommandHandler
    {
    }

    public interface ICommandHandler<in TCommand> : ICommandHandler
    {
        void Handle(TCommand command);
    }

    public interface ICommandHandler<in TCommand, out TResult> : ICommandHandler
        where TCommand : ICommand
        where TResult : ICommandResult
    {
        TResult Handle(TCommand command);
    }
}