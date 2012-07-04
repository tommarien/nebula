using System.Web.Mvc;
using NUnit.Framework;
using Nebula.Infrastructure.Querying;
using Nebula.MvcApplication.Controllers;
using Rhino.Mocks;

namespace Nebula.UnitTests.Nebula.MvcApplication.Controllers.System
{
    [TestFixture]
    public class SystemAboutGetTests : HttpContextFixture
    {
        private SystemController controller;

        protected override void AfterSetUp()
        {
            controller = new SystemController(MockRepository.GenerateStrictMock<IQueryHandlerFactory>());
            controller.ControllerContext = new ControllerContext(HttpContext, RouteData, controller);
        }

        [Test]
        public void Should_return_viewResult()
        {
            var result = controller.About();

            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}