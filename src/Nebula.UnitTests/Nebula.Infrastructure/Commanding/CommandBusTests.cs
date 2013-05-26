using System.Data;
using NUnit.Framework;
using Nebula.Infrastructure.Commanding;
using Rhino.Mocks;

namespace Nebula.UnitTests.Nebula.Infrastructure.Commanding
{
    [TestFixture]
    public class CommandBusTests
    {
        [SetUp]
        public void Setup()
        {
            commandHandlerFactory = MockRepository.GenerateMock<ICommandHandlerFactory>();
            commandBus = new CommandBus(commandHandlerFactory);
        }

        private ICommandHandlerFactory commandHandlerFactory;
        private CommandBus commandBus;


        [Test]
        public void Send_Should_get_an_instance_of_the_commandhandler_from_the_factory()
        {
            commandHandlerFactory.Expect(f => f.CreateHandler<TestCommand>()).Return(new TestCommandHandler()).Message(
                "The commandhandler was not requested from the factory");

            commandBus.Send(new TestCommand());

            commandHandlerFactory.VerifyAllExpectations();
        }

        [Test]
        public void Send_Should_invoke_the_commandhandler()
        {
            var commandhandler = MockRepository.GenerateStrictMock<ICommandHandler<TestCommand>>();
            commandHandlerFactory.Stub(f => f.CreateHandler<TestCommand>()).Return(commandhandler);

            var command = new TestCommand();
            commandhandler.Expect(h => h.Handle(command))
                          .Message("The commandhandler was not invoked in the expected way");

            commandBus.Send(command);

            commandhandler.VerifyAllExpectations();
        }

        [Test]
        public void Send_Should_release_the_commandhandler()
        {
            var commandhandler = MockRepository.GenerateMock<ICommandHandler<TestCommand>>();
            commandHandlerFactory.Stub(f => f.CreateHandler<TestCommand>()).Return(commandhandler);

            commandHandlerFactory.Expect(f => f.ReleaseHandler(commandhandler))
                                 .Message("The commandhandler was not released");

            commandBus.Send(new TestCommand());

            commandHandlerFactory.VerifyAllExpectations();
        }

        [Test]
        public void Send_Should_release_the_commandhandler_no_matter_what()
        {
            var commandhandler = MockRepository.GenerateMock<ICommandHandler<TestCommand>>();
            commandhandler.Stub(h => h.Handle(new TestCommand()))
                          .IgnoreArguments()
                          .Throw(new InvalidExpressionException());
            commandHandlerFactory.Stub(f => f.CreateHandler<TestCommand>()).Return(commandhandler);

            commandHandlerFactory.Expect(f => f.ReleaseHandler(commandhandler))
                                 .Message("The commandhandler was not released when it throws an exception");

            Assert.Catch(() => commandBus.Send(new TestCommand()));

            commandHandlerFactory.VerifyAllExpectations();
        }
    }
}