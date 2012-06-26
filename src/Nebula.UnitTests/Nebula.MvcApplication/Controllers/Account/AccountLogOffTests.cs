using System.Web.Mvc;
using NUnit.Framework;
using Nebula.Contracts.Registration.Queries;
using Nebula.Infrastructure.Commanding;
using Nebula.MvcApplication.Controllers;
using Nebula.MvcApplication.Services;
using Rhino.Mocks;

namespace Nebula.UnitTests.Nebula.MvcApplication.Controllers.Account
{
    [TestFixture]
    public class AccountLogOffTests : HttpContextFixture
    {
        private AccountController controller;
        private ICommandDispatcher commandDispatcher;
        private IFormsAuthenticationService formsAuthenticationService;

        protected override void AfterSetUp()
        {
            commandDispatcher = MockRepository.GenerateStub<ICommandDispatcher>();
            formsAuthenticationService = MockRepository.GenerateMock<IFormsAuthenticationService>();

            controller = new AccountController(commandDispatcher, formsAuthenticationService, MockRepository.GenerateStub<IGetAccountRolesQuery>());
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