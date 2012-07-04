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
    public class AccountLogOnGetTests : HttpContextFixture
    {
        private AccountController controller;
        private ICommandDispatcher commandDispatcher;
        private IFormsAuthenticationService formsAuthenticationService;

        protected override void AfterSetUp()
        {
            commandDispatcher = MockRepository.GenerateStrictMock<ICommandDispatcher>();
            formsAuthenticationService = MockRepository.GenerateStrictMock<IFormsAuthenticationService>();
            controller = new AccountController(commandDispatcher, formsAuthenticationService, MockRepository.GenerateStrictMock<IGetAccountRolesQueryHandler>());
            controller.ControllerContext = new ControllerContext(HttpContext, RouteData, controller);
        }

        [Test]
        public void Should_return_viewResult()
        {
            var result = controller.LogOn();

            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}