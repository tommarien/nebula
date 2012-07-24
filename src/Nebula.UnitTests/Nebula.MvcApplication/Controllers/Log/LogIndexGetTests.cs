using System.Web.Mvc;
using NUnit.Framework;
using Nebula.Infrastructure.Querying;
using Nebula.MvcApplication.Controllers;
using Rhino.Mocks;

namespace Nebula.UnitTests.Nebula.MvcApplication.Controllers.Log
{
    [TestFixture]
    public class LogIndexGetTests : HttpContextFixture
    {
        private LogController controller;

        protected override void AfterSetUp()
        {
            controller = new LogController(MockRepository.GenerateStrictMock<IQueryHandlerFactory>());
            SetupControllerContext(controller);
        }

        [Test]
        public void Should_return_viewResult()
        {
            var result = controller.Index();

            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}