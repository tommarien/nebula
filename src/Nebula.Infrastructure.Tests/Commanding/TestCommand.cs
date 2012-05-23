using Nebula.Infrastructure.Commanding;

namespace Nebula.Infrastructure.Tests.Commanding
{
    public class TestCommand : ICommand
    {
        public int Expected { get; set; }
    }

    public class TestCommandHandler : ICommandHandler<TestCommand>
    {
        public ICommandResult Handle(TestCommand instance)
        {
            return new SimpleCommandResult<int>(instance.Expected);
        }
    }
}