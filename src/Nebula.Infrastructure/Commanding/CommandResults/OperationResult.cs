namespace Nebula.Infrastructure.Commanding.CommandResults
{
    public class OperationResult : ICommandResult
    {
        public OperationResult(bool success)
        {
            Success = success;
        }

        public bool Success { get; private set; }

        public static implicit operator OperationResult(bool value)
        {
            return new OperationResult(value);
        }

        public static implicit operator bool(OperationResult instance)
        {
            return instance.Success;
        }
    }
}