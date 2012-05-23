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
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            windsorContainer = new WindsorContainer();
            windsorContainer.Install(new CommandingInstaller());
            windsorContainer.Register(Component.For<ICommandHandler<TestCommand>>().ImplementedBy<TestCommandHandler>().LifestyleTransient());
        }

        [TearDown]
        public void TearDown()
        {
            windsorContainer.Dispose();
        }

        #endregion

        private IWindsorContainer windsorContainer;

        [Test]
        public void Should_be_able_to_createhandler()
        {
            var handlerFactory = windsorContainer.Resolve<ICommandHandlerFactory>();

            Assert.IsInstanceOf<TestCommandHandler>(handlerFactory.CreateHandler<TestCommand>());
        }

        [Test]
        public void Should_be_able_to_resolve_commandexecutor()
        {
            Assert.IsInstanceOf<CommandExecutor>(windsorContainer.Resolve<ICommandExecutor>());
        }
    }
}