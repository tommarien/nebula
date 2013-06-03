using System.Linq;
using System.Text;
using Castle.MicroKernel;
using Castle.MicroKernel.Handlers;
using Castle.Windsor;
using Castle.Windsor.Diagnostics;
using Castle.Windsor.Installer;
using NUnit.Framework;
using Nebula.Bootstrapper;
using Nebula.MvcApplication.Services;

namespace Nebula.IntegrationTests
{
    [TestFixture]
    public class WindsorTests
    {
        [SetUp]
        public void Setup()
        {
            container = Boot.Container()
                .Install(FromAssembly.Containing<IFormsAuthenticationService>());
        }

        [TearDown]
        public void TearDown()
        {
            container.Dispose();
        }

        private IWindsorContainer container;

        [Test]
        public void ShouldNotHaveAnyMisconfiguredComponents()
        {
            var diagnostic
             = new PotentiallyMisconfiguredComponentsDiagnostic(container.Kernel);

            IHandler[] handlers = diagnostic.Inspect();
            if (handlers != null && handlers.Any())
            {
                var builder = new StringBuilder();

                builder.AppendFormat("Potentially misconfigured components ({0})\r\n", handlers.Count());

                foreach (IHandler handler in handlers)
                {
                    var info = (IExposeDependencyInfo)handler;
                    var inspector = new DependencyInspector(builder);
                    info.ObtainDependencyDetails(inspector);
                }

                Assert.Inconclusive(builder.ToString());
            }
        }
    }
}