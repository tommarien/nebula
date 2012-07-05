using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using Rhino.Mocks;

namespace Nebula.UnitTests.Nebula.MvcApplication
{
    public abstract class HttpContextFixture
    {
        protected HttpContextBase HttpContext { get; private set; }
        protected HttpRequestBase HttpRequest { get; private set; }
        protected RouteData RouteData { get; private set; }

        [SetUp]
        public void Setup()
        {
            BeforeSetUp();

            HttpContext = MockRepository.GenerateMock<HttpContextBase>();
            HttpRequest = MockRepository.GenerateMock<HttpRequestBase>();

            RouteData = new RouteData();

            HttpContext.Stub(c => c.Request).Return(HttpRequest);

            AfterSetUp();
        }

        protected virtual void BeforeSetUp()
        {
        }

        protected virtual void AfterSetUp()
        {
        }

        protected void SetupControllerContext(ControllerBase controller)
        {
            controller.ControllerContext = new ControllerContext(HttpContext, RouteData, controller);
        }
    }
}