using System.Data;
using NUnit.Framework;
using Nebula.Infrastructure.Commanding;
using Rhino.Mocks;

namespace Nebula.Infrastructure.Tests.Commanding
{
    [TestFixture]
    public class CommandExecutorTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            commandHandlerFactory = MockRepository.GenerateMock<ICommandHandlerFactory>();
            commandExecutor = new CommandExecutor(commandHandlerFactory);
        }

        #endregion

        private ICommandHandlerFactory commandHandlerFactory;
        private CommandExecutor commandExecutor;


        [Test]
        public void Execute_Should_get_an_instance_of_the_commandhandler_from_the_factory()
        {
            commandHandlerFactory.Expect(f => f.CreateHandler<TestCommand>()).Return(new TestCommandHandler()).Message(
                "The commandhandler was not requested from the factory");

            commandExecutor.Execute(new TestCommand());

            commandHandlerFactory.VerifyAllExpectations();
        }

        [Test]
        public void Execute_Should_invoke_the_commandhandler_and_return_its_result()
        {
            var commandhandler = MockRepository.GenerateStrictMock<ICommandHandler<TestCommand>>();
            commandHandlerFactory.Stub(f => f.CreateHandler<TestCommand>()).Return(commandhandler);

            var command = new TestCommand();
            var commandResult = MockRepository.GenerateMock<ICommandResult>();
            commandhandler.Expect(h => h.Handle(command)).Return(commandResult).Message("The commandhandler was not invoked in the expected way");

            Assert.AreEqual(commandResult, commandExecutor.Execute(command));

            commandhandler.VerifyAllExpectations();
        }

        [Test]
        public void Execute_Should_release_the_commandhandler()
        {
            var commandhandler = MockRepository.GenerateMock<ICommandHandler<TestCommand>>();
            commandHandlerFactory.Stub(f => f.CreateHandler<TestCommand>()).Return(commandhandler);

            commandHandlerFactory.Expect(f => f.ReleaseHandler(commandhandler)).Message("The commandhandler was not released");

            commandExecutor.Execute(new TestCommand());

            commandHandlerFactory.VerifyAllExpectations();
        }

        [Test]
        public void Execute_Should_release_the_commandhandler_no_matter_what()
        {
            var commandhandler = MockRepository.GenerateMock<ICommandHandler<TestCommand>>();
            commandhandler.Stub(h => h.Handle(new TestCommand())).IgnoreArguments().Throw(new InvalidExpressionException());
            commandHandlerFactory.Stub(f => f.CreateHandler<TestCommand>()).Return(commandhandler);

            commandHandlerFactory.Expect(f => f.ReleaseHandler(commandhandler)).Message("The commandhandler was not released when it throws an exception");

            Assert.Catch(() => commandExecutor.Execute(new TestCommand()));

            commandHandlerFactory.VerifyAllExpectations();
        }
    }
}