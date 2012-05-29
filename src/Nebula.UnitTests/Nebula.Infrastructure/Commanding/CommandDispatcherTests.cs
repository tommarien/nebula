using System.Data;
using NUnit.Framework;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Commanding.CommandResults;
using Rhino.Mocks;

namespace Nebula.UnitTests.Nebula.Infrastructure.Commanding
{
    [TestFixture]
    public class CommandDispatcherTests
    {
        [SetUp]
        public void Setup()
        {
            commandHandlerFactory = MockRepository.GenerateMock<ICommandHandlerFactory>();
            commandDispatcher = new CommandDispatcher(commandHandlerFactory);
        }

        private ICommandHandlerFactory commandHandlerFactory;
        private CommandDispatcher commandDispatcher;

        [Test]
        public void DispatchWithResult_Should_get_an_instance_of_the_commandhandler_from_the_factory()
        {
            commandHandlerFactory.Expect(f => f.CreateHandler<TestCommandWithResult, OperationResult>()).Return(new TestCommandWithResultHandler())
                .Message("The commandhandler was not requested from the factory");

            commandDispatcher.Dispatch<TestCommandWithResult, OperationResult>(new TestCommandWithResult());

            commandHandlerFactory.VerifyAllExpectations();
        }

        [Test]
        public void DispatchWithResult_Should_invoke_the_commandhandler_and_return_result()
        {
            var commandhandler = MockRepository.GenerateStrictMock<ICommandHandler<TestCommand, OperationResult>>();
            commandHandlerFactory.Stub(f => f.CreateHandler<TestCommand, OperationResult>()).Return(commandhandler);

            var command = new TestCommand();
            commandhandler.Expect(h => h.Handle(command)).Return(new OperationResult(true)).Message("The commandhandler was not invoked in the expected way");

            var result = commandDispatcher.Dispatch<TestCommand, OperationResult>(command);

            Assert.IsTrue(result);

            commandhandler.VerifyAllExpectations();
        }

        [Test]
        public void DispatchWithResult_Should_release_the_commandhandler()
        {
            var commandhandler = MockRepository.GenerateMock<ICommandHandler<TestCommand, OperationResult>>();
            commandHandlerFactory.Stub(f => f.CreateHandler<TestCommand, OperationResult>()).Return(commandhandler);

            commandHandlerFactory.Expect(f => f.ReleaseHandler(commandhandler)).Message("The commandhandler was not released");

            commandDispatcher.Dispatch<TestCommand, OperationResult>(new TestCommand());

            commandHandlerFactory.VerifyAllExpectations();
        }

        [Test]
        public void DispatchWithResult_Should_release_the_commandhandler_no_matter_what()
        {
            var commandhandler = MockRepository.GenerateMock<ICommandHandler<TestCommand, OperationResult>>();
            commandhandler.Stub(h => h.Handle(new TestCommand())).IgnoreArguments().Throw(new InvalidExpressionException());
            commandHandlerFactory.Stub(f => f.CreateHandler<TestCommand, OperationResult>()).Return(commandhandler);

            commandHandlerFactory.Expect(f => f.ReleaseHandler(commandhandler)).Message("The commandhandler was not released when it throws an exception");

            Assert.Catch(() => commandDispatcher.Dispatch<TestCommand, OperationResult>(new TestCommand()));

            commandHandlerFactory.VerifyAllExpectations();
        }

        [Test]
        public void Dispatch_Should_get_an_instance_of_the_commandhandler_from_the_factory()
        {
            commandHandlerFactory.Expect(f => f.CreateHandler<TestCommand>()).Return(new TestCommandHandler()).Message(
                "The commandhandler was not requested from the factory");

            commandDispatcher.Dispatch(new TestCommand());

            commandHandlerFactory.VerifyAllExpectations();
        }

        [Test]
        public void Dispatch_Should_invoke_the_commandhandler()
        {
            var commandhandler = MockRepository.GenerateStrictMock<ICommandHandler<TestCommand>>();
            commandHandlerFactory.Stub(f => f.CreateHandler<TestCommand>()).Return(commandhandler);

            var command = new TestCommand();
            commandhandler.Expect(h => h.Handle(command)).Message("The commandhandler was not invoked in the expected way");

            commandDispatcher.Dispatch(command);

            commandhandler.VerifyAllExpectations();
        }

        [Test]
        public void Dispatch_Should_release_the_commandhandler()
        {
            var commandhandler = MockRepository.GenerateMock<ICommandHandler<TestCommand>>();
            commandHandlerFactory.Stub(f => f.CreateHandler<TestCommand>()).Return(commandhandler);

            commandHandlerFactory.Expect(f => f.ReleaseHandler(commandhandler)).Message("The commandhandler was not released");

            commandDispatcher.Dispatch(new TestCommand());

            commandHandlerFactory.VerifyAllExpectations();
        }

        [Test]
        public void Dispatch_Should_release_the_commandhandler_no_matter_what()
        {
            var commandhandler = MockRepository.GenerateMock<ICommandHandler<TestCommand>>();
            commandhandler.Stub(h => h.Handle(new TestCommand())).IgnoreArguments().Throw(new InvalidExpressionException());
            commandHandlerFactory.Stub(f => f.CreateHandler<TestCommand>()).Return(commandhandler);

            commandHandlerFactory.Expect(f => f.ReleaseHandler(commandhandler)).Message("The commandhandler was not released when it throws an exception");

            Assert.Catch(() => commandDispatcher.Dispatch(new TestCommand()));

            commandHandlerFactory.VerifyAllExpectations();
        }
    }
}