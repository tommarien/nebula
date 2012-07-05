using System.Web.Mvc;
using NUnit.Framework;
using Nebula.Infrastructure.Querying;
using Nebula.MvcApplication.Controllers;
using Rhino.Mocks;

namespace Nebula.UnitTests.Nebula.MvcApplication.Controllers.System
{
    [TestFixture]
    public class SystemLoggingGetTests : HttpContextFixture
    {
        private SystemController controller;

        protected override void AfterSetUp()
        {
            controller = new SystemController(MockRepository.GenerateStrictMock<IQueryHandlerFactory>());
            SetupControllerContext(controller);
        }

        [Test]
        public void Should_return_viewResult()
        {
            var result = controller.Logging();

            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}