using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Commanding.CommandResults;

namespace Nebula.Infrastructure.Tests.Commanding
{
    public class TestCommand : ICommand
    {
    }

    public class TestCommandHandler : ICommandHandler<TestCommand>
    {
        public void Handle(TestCommand instance)
        {
        }
    }

    public class TestCommandWithResult : ICommand
    {
    }

    public class TestCommandWithResultHandler : ICommandHandler<TestCommandWithResult, OperationResult>
    {
        public OperationResult Handle(TestCommandWithResult command)
        {
            return false;
        }
    }
}