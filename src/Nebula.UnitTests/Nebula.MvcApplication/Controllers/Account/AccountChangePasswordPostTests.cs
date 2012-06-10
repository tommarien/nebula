using System;
using System.Security.Principal;
using System.Web.Mvc;
using NUnit.Framework;
using Nebula.Contracts.Registration;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Commanding.CommandResults;
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
            HttpContext.Stub(c => c.User).Return(new GenericPrincipal(new GenericIdentity("userX"), new string[] {}));
        }

        [Test]
        public void Should_add_an_error_and_return_view_anything_goes_wrong()
        {
            commandDispatcher.Stub(d => d.Dispatch<ChangePasswordCommand, OperationResult>(Arg<ChangePasswordCommand>.Is.Anything))
                .Throw(new Exception());

            var result = controller.ChangePassword(model);

            Assert.IsFalse(controller.ModelState.IsValid);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreSame(model, ((ViewResult) result).Model);
        }

        [Test]
        public void Should_add_an_error_and_return_view_if_the_command_returns_false()
        {
            commandDispatcher.Stub(d => d.Dispatch<ChangePasswordCommand, OperationResult>(Arg<ChangePasswordCommand>.Is.Anything)).Return(false);

            var result = controller.ChangePassword(model);

            Assert.IsFalse(controller.ModelState.IsValid);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreSame(model, ((ViewResult) result).Model);
        }

        [Test]
        public void Should_dispatch_the_expected_command()
        {
            commandDispatcher.Expect(d => d.Dispatch<ChangePasswordCommand, OperationResult>(
                Arg<ChangePasswordCommand>.Matches(c => c.UserName == "userX"
                                                        && c.OldPassword == model.OldPassword
                                                        && c.NewPassword == model.NewPassword)))
                .Return(false);

            controller.ChangePassword(model);

            commandDispatcher.VerifyAllExpectations();
        }

        [Test]
        public void Should_return_ChangePasswordSuccess_if_succeeds()
        {
            commandDispatcher.Stub(d => d.Dispatch<ChangePasswordCommand, OperationResult>(Arg<ChangePasswordCommand>.Is.Anything))
                .Return(true);

            var result = controller.ChangePassword(model);

            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.AreEqual("ChangePasswordSuccess", ((RedirectToRouteResult) result).RouteValues["action"]);
            Assert.IsNull(((RedirectToRouteResult) result).RouteValues["controller"]);
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