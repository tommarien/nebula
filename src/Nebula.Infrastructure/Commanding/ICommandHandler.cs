namespace Nebula.Infrastructure.Commanding
{
    public interface ICommandHandler<T> : IHandle<T> where T : ICommand
    {
    }
}