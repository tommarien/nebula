namespace Nebula.Infrastructure.Commanding
{
    public interface ICommandDispatcher
    {
        ICommandResult Dispatch<T>(T command) where T : ICommand;
    }
}