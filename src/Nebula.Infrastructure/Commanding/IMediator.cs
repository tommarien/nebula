namespace Nebula.Infrastructure.Commanding
{
    public interface IMediator
    {
        void Execute<TCommand>(TCommand command) where TCommand : ICommand;
        TResult Execute<TCommand, TResult>(TCommand command) where TCommand : ICommand;
    }
}