using System;
using Nebula.Infrastructure.Commanding;

namespace Nebula.Infrastructure.Tests.Commanding
{
    public class TestCommand : ICommand
    {
    }

    public class TestCommandHandler : ICommandHandler<TestCommand>, IDisposable
    {
        public void Handle(TestCommand instance)
        {
        }

        public void Dispose()
        {
        }
    }
}