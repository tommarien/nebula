using System.Web.Mvc;
using NUnit.Framework;
using Nebula.Infrastructure.Querying;
using Nebula.MvcApplication.Controllers;
using Rhino.Mocks;

namespace Nebula.UnitTests.Nebula.MvcApplication.Controllers.System
{
    [TestFixture]
    public class SystemIndexGetTests : HttpContextFixture
    {
        private SystemController controller;

        protected override void AfterSetUp()
        {
            controller = new SystemController(MockRepository.GenerateStrictMock<IQueryFactory>());
            controller.ControllerContext = new ControllerContext(HttpContext, RouteData, controller);
        }

        [Test]
        public void Should_return_viewResult()
        {
            var result = controller.Index();

            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}