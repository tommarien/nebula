using System.Web.Mvc;
using NUnit.Framework;
using Nebula.Infrastructure.Commanding;
using Nebula.MvcApplication.Controllers;
using Nebula.MvcApplication.Services;
using Rhino.Mocks;

namespace Nebula.UnitTests.Nebula.MvcApplication.Controllers.Account
{
    [TestFixture]
    public class Account_LogOffTests : HttpContextFixture
    {
        private AccountController controller;
        private ICommandDispatcher commandDispatcher;
        private IFormsAuthenticationService formsAuthenticationService;

        protected override void AfterSetUp()
        {
            commandDispatcher = MockRepository.GenerateMock<ICommandDispatcher>();
            formsAuthenticationService = MockRepository.GenerateMock<IFormsAuthenticationService>();
            controller = new AccountController(commandDispatcher, formsAuthenticationService);
            controller.ControllerContext = new ControllerContext(HttpContext, RouteData, controller);
        }

        [Test]
        public void Should_Invoke_SignOut()
        {
            formsAuthenticationService.Expect(s => s.SignOut());

            controller.LogOff();

            formsAuthenticationService.VerifyAllExpectations();
        }

        [Test]
        public void Should_redirect_to_HomePage()
        {
            var result = controller.LogOff();

            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            var redirectToRouteResult = (RedirectToRouteResult) result;
            Assert.AreEqual("Home", redirectToRouteResult.RouteValues["controller"]);
            Assert.AreEqual("Index", redirectToRouteResult.RouteValues["action"]);
        }
    }
}