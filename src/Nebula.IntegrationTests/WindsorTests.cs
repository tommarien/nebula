using System.Linq;
using System.Text;
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
            var diagnostic = new PotentiallyMisconfiguredComponentsDiagnostic(container.Kernel);
            var handlers = diagnostic.Inspect();
            if (handlers != null && handlers.Any())
            {
                var builder = new StringBuilder();

                builder.AppendFormat("Misconfigured components ({0})\r\n", handlers.Count());

                handlers
                    .ToList()
                    .ForEach(h => builder.AppendFormat("=> {0} is {1}\r\n", h.ToString(), h.CurrentState));

                Assert.Fail(builder.ToString());
            }
        }
    }
}