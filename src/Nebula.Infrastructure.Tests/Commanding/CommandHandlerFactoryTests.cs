using Castle.Windsor;
using NUnit.Framework;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Installers;

namespace Nebula.Infrastructure.Tests.Commanding
{
    [TestFixture]
    public class CommandHandlerFactoryTests
    {
        [SetUp]
        public void Setup()
        {
            container = new WindsorContainer();
            container.Install(new CommandingInstaller());
            container.RegisterCommandHandlers(typeof (TestCommand).Assembly);
        }

        [TearDown]
        public void TearDown()
        {
            container.Dispose();
        }

        private IWindsorContainer container;

        [Test]
        public void CreateHandler_Should_resolve_the_expected_handler()
        {
            var factory = container.Resolve<ICommandHandlerFactory>();

            var handler = factory.CreateHandler<TestCommand>();
            Assert.IsInstanceOf<TestCommandHandler>(handler);
        }

        [Test]
        public void ReleaseHandler_Should_release_handler_from_container()
        {
            var factory = container.Resolve<ICommandHandlerFactory>();

            var handler = factory.CreateHandler<TestCommand>();

            factory.ReleaseHandler(handler);
            Assert.IsFalse(container.Kernel.ReleasePolicy.HasTrack(handler));
        }
    }
}