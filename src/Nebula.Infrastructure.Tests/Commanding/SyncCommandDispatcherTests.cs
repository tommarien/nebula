using System;
using NUnit.Framework;
using Nebula.Infrastructure.Commanding;
using Rhino.Mocks;

namespace Nebula.Infrastructure.Tests.Commanding
{
    [TestFixture]
    public class SyncCommandDispatcherTests
    {
        [SetUp]
        public void Setup()
        {
            commandHandlerFactory = MockRepository.GenerateStub<ICommandHandlerFactory>();
            dispatcher = new SyncCommandDispatcher(commandHandlerFactory);
        }

        private ICommandHandlerFactory commandHandlerFactory;
        private ICommandDispatcher dispatcher;

        [Test]
        public void Dispatch_Should_call_CreateHandler()
        {
            var handlerMock = MockRepository.GenerateStub<ICommandHandler<TestCommand>>();

            commandHandlerFactory.Expect(f => f.CreateHandler<TestCommand>()).Return(handlerMock);

            dispatcher.Dispatch(new TestCommand());

            commandHandlerFactory.VerifyAllExpectations();
        }

        [Test]
        public void Dispatch_Should_call_handle_with_specified_command()
        {
            var handlerMock = MockRepository.GenerateStrictMock<ICommandHandler<TestCommand>>();
            commandHandlerFactory.Stub(f => f.CreateHandler<TestCommand>()).Return(handlerMock);

            var command = new TestCommand();

            handlerMock.Expect(h => h.Handle(command));

            dispatcher.Dispatch(command);

            handlerMock.VerifyAllExpectations();
        }

        [Test]
        public void Dispatch_Should_release_handler()
        {
            var handlerMock = MockRepository.GenerateStub<ICommandHandler<TestCommand>>();

            commandHandlerFactory.Stub(f => f.CreateHandler<TestCommand>()).Return(handlerMock);
            commandHandlerFactory.Expect(f => f.ReleaseHandler(handlerMock));

            dispatcher.Dispatch(new TestCommand());

            commandHandlerFactory.VerifyAllExpectations();
        }

        [Test]
        public void Dispatch_Should_release_handler_even_if_it_throws()
        {
            var handlerMock = MockRepository.GenerateStub<ICommandHandler<TestCommand>>();
            commandHandlerFactory.Stub(f => f.CreateHandler<TestCommand>()).Return(handlerMock);

            handlerMock.Stub(h => h.Handle(new TestCommand())).IgnoreArguments().Throw(new ArgumentNullException());

            commandHandlerFactory.Expect(f => f.ReleaseHandler(handlerMock));

            Assert.Throws<ArgumentNullException>(() => dispatcher.Dispatch(new TestCommand()));

            commandHandlerFactory.VerifyAllExpectations();
        }
    }
}