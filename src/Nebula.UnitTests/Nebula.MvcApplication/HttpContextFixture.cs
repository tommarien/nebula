using System.Web;
using System.Web.Routing;
using NUnit.Framework;
using Rhino.Mocks;

namespace Nebula.UnitTests.Nebula.MvcApplication
{
    public abstract class HttpContextFixture
    {
        protected HttpContextBase HttpContext { get; private set; }
        protected RouteData RouteData { get; private set; }

        [SetUp]
        public void Setup()
        {
            BeforeSetUp();

            HttpContext = MockRepository.GenerateMock<HttpContextBase>();
            RouteData = new RouteData();

            AfterSetUp();
        }

        protected virtual void BeforeSetUp()
        {
        }

        protected virtual void AfterSetUp()
        {
        }
    }
}