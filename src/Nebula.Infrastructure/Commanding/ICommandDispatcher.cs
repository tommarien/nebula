namespace Nebula.Infrastructure.Commanding
{
    public interface ICommandDispatcher
    {
        void Dispatch<T>(T command) where T : ICommand;
    }
}