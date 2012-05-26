using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Tests.Commanding;

namespace Nebula.Infrastructure.Tests
{
    [TestFixture]
    public class CommandingInstallerTests
    {
        [SetUp]
        public void Setup()
        {
            windsorContainer = new WindsorContainer();
            windsorContainer.Install(new CommandingInstaller());
        }

        [TearDown]
        public void TearDown()
        {
            windsorContainer.Dispose();
        }

        private IWindsorContainer windsorContainer;

        [Test]
        public void Should_be_able_to_resolve_commandHandlerFactory()
        {
            Assert.IsInstanceOf<ICommandHandlerFactory>(windsorContainer.Resolve<ICommandHandlerFactory>());
        }

        [Test]
        public void Should_be_able_to_resolve_commandexecutor()
        {
            Assert.IsInstanceOf<CommandExecutor>(windsorContainer.Resolve<ICommandExecutor>());
        }
    }
}