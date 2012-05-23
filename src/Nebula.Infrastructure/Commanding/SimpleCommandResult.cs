namespace Nebula.Infrastructure.Commanding
{
    public class SimpleCommandResult<T> : ICommandResult
    {
        public SimpleCommandResult(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }
    }
}