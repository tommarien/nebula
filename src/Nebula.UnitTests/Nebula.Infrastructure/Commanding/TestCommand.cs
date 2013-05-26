using Nebula.Infrastructure.Commanding;

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
}