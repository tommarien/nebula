using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Commanding.CommandResults;

namespace Nebula.UnitTests.Nebula.Infrastructure.Commanding
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