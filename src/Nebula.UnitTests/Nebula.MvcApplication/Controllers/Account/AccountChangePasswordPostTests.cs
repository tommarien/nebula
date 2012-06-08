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
    public class AccountChangePasswordPostTests : HttpContextFixture
    {
        private AccountController controller;
        private ICommandDispatcher commandDispatcher;
        private IFormsAuthenticationService formsAuthenticationService;
        private ChangePasswordModel model;

        protected override void AfterSetUp()
        {
            commandDispatcher = MockRepository.GenerateMock<ICommandDispatcher>();
            formsAuthenticationService = MockRepository.GenerateMock<IFormsAuthenticationService>();
            controller = new AccountController(commandDispatcher, formsAuthenticationService);
            controller.ControllerContext = new ControllerContext(HttpContext, RouteData, controller);
            model = new ChangePasswordModel {OldPassword = "oldsecret", NewPassword = "newsecret", ConfirmPassword = "newsecret"};
        }

        [Test]
        public void Should_return_view_if_model_is_invalid()
        {
            controller.ModelState.AddModelError("", "Something is wrong");

            var result = controller.ChangePassword(model);

            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreSame(model, ((ViewResult) result).Model);
        }
    }
}