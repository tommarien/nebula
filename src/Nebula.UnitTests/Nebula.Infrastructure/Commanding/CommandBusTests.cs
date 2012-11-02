using System.Data;
using NUnit.Framework;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Commanding.CommandResults;
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
        public void SendAndReply_Should_get_an_instance_of_the_commandhandler_from_the_factory()
        {
            commandHandlerFactory.Expect(f => f.CreateHandler<TestCommandWithResult, OperationResult>()).Return(new TestCommandWithResultHandler())
                .Message("The commandhandler was not requested from the factory");

            commandBus.SendAndReply<TestCommandWithResult, OperationResult>(new TestCommandWithResult());

            commandHandlerFactory.VerifyAllExpectations();
        }

        [Test]
        public void SendAndReply_Should_invoke_the_commandhandler_and_return_result()
        {
            var commandhandler = MockRepository.GenerateStrictMock<ICommandHandler<TestCommand, OperationResult>>();
            commandHandlerFactory.Stub(f => f.CreateHandler<TestCommand, OperationResult>()).Return(commandhandler);

            var command = new TestCommand();
            commandhandler.Expect(h => h.Handle(command)).Return(new OperationResult(true)).Message("The commandhandler was not invoked in the expected way");

            var result = commandBus.SendAndReply<TestCommand, OperationResult>(command);

            Assert.IsTrue(result);

            commandhandler.VerifyAllExpectations();
        }

        [Test]
        public void SendAndReply_Should_release_the_commandhandler()
        {
            var commandhandler = MockRepository.GenerateMock<ICommandHandler<TestCommand, OperationResult>>();
            commandHandlerFactory.Stub(f => f.CreateHandler<TestCommand, OperationResult>()).Return(commandhandler);

            commandHandlerFactory.Expect(f => f.ReleaseHandler(commandhandler)).Message("The commandhandler was not released");

            commandBus.SendAndReply<TestCommand, OperationResult>(new TestCommand());

            commandHandlerFactory.VerifyAllExpectations();
        }

        [Test]
        public void SendAndReply_Should_release_the_commandhandler_no_matter_what()
        {
            var commandhandler = MockRepository.GenerateMock<ICommandHandler<TestCommand, OperationResult>>();
            commandhandler.Stub(h => h.Handle(new TestCommand())).IgnoreArguments().Throw(new InvalidExpressionException());
            commandHandlerFactory.Stub(f => f.CreateHandler<TestCommand, OperationResult>()).Return(commandhandler);

            commandHandlerFactory.Expect(f => f.ReleaseHandler(commandhandler)).Message("The commandhandler was not released when it throws an exception");

            Assert.Catch(() => commandBus.SendAndReply<TestCommand, OperationResult>(new TestCommand()));

            commandHandlerFactory.VerifyAllExpectations();
        }

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
            commandhandler.Expect(h => h.Handle(command)).Message("The commandhandler was not invoked in the expected way");

            commandBus.Send(command);

            commandhandler.VerifyAllExpectations();
        }

        [Test]
        public void Send_Should_release_the_commandhandler()
        {
            var commandhandler = MockRepository.GenerateMock<ICommandHandler<TestCommand>>();
            commandHandlerFactory.Stub(f => f.CreateHandler<TestCommand>()).Return(commandhandler);

            commandHandlerFactory.Expect(f => f.ReleaseHandler(commandhandler)).Message("The commandhandler was not released");

            commandBus.Send(new TestCommand());

            commandHandlerFactory.VerifyAllExpectations();
        }

        [Test]
        public void Send_Should_release_the_commandhandler_no_matter_what()
        {
            var commandhandler = MockRepository.GenerateMock<ICommandHandler<TestCommand>>();
            commandhandler.Stub(h => h.Handle(new TestCommand())).IgnoreArguments().Throw(new InvalidExpressionException());
            commandHandlerFactory.Stub(f => f.CreateHandler<TestCommand>()).Return(commandhandler);

            commandHandlerFactory.Expect(f => f.ReleaseHandler(commandhandler)).Message("The commandhandler was not released when it throws an exception");

            Assert.Catch(() => commandBus.Send(new TestCommand()));

            commandHandlerFactory.VerifyAllExpectations();
        }
    }
}