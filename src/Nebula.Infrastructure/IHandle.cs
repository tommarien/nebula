namespace Nebula.Infrastructure
{
    public interface IHandle<T>
    {
        void Handle(T instance);
    }

    public interface IHandleAndReply<TInput, TOutput>
    {
        TOutput Handle(TInput instance);
    }
}