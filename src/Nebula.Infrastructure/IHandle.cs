namespace Nebula.Infrastructure
{
    public interface IHandle<T>
    {
        void Handle(T instance);
    }
}