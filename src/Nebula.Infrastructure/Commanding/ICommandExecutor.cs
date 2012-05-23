namespace Nebula.Infrastructure.Commanding
{
    public interface ICommandExecutor
    {
        ICommandResult Execute<T>(T command) where T : ICommand;
    }
}