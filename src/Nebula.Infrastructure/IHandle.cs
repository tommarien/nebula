namespace Nebula.Infrastructure
{
    public interface IHandle<in T>
    {
        void Handle(T instance);
    }

    public interface IHandleAndReply<in TInput, out TOutput>
    {
        TOutput Handle(TInput instance);
    }
}