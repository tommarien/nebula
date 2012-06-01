using System.Web.Mvc;
using NUnit.Framework;
using Nebula.Infrastructure.Commanding;
using Nebula.MvcApplication.Controllers;
using Nebula.MvcApplication.Models;
using Nebula.MvcApplication.Services;
using Rhino.Mocks;

namespace Nebula.UnitTests.Nebula.MvcApplication.Controllers.Account
{
    [TestFixture]
    public class AccountLogOnPostTests : HttpContextFixture
    {
        private AccountController controller;
        private ICommandDispatcher commandDispatcher;
        private IFormsAuthenticationService formsAuthenticationService;
        private LogOnModel logOnModel;

        protected override void AfterSetUp()
        {
            commandDispatcher = MockRepository.GenerateStrictMock<ICommandDispatcher>();
            formsAuthenticationService = MockRepository.GenerateStrictMock<IFormsAuthenticationService>();
            controller = new AccountController(commandDispatcher, formsAuthenticationService);
            controller.ControllerContext = new ControllerContext(HttpContext, RouteData, controller);
            logOnModel = new LogOnModel {UserName = "userX", Password = "secret", RememberMe = true};
        }

        [Test]
        public void Should_return_view_if_model_is_invalid()
        {
            controller.ModelState.AddModelError("", "Something is wrong");

            var result = controller.LogOn(logOnModel, "/");

            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual(string.Empty, ((ViewResult) result).ViewName);
            Assert.AreSame(logOnModel, ((ViewResult) result).Model);
        }
    }
}