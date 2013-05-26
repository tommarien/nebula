namespace Nebula.Infrastructure.Commanding
{
    public interface ICommandBus
    {
        void Send<TCommand>(TCommand command)
            where TCommand : ICommand;
    }
}