using System;
using System.Web.Mvc;
using System.Web.Routing;
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
    public class AccountLogOnPostTests : HttpContextFixture
    {
        private AccountController controller;
        private ICommandDispatcher commandDispatcher;
        private IFormsAuthenticationService formsAuthenticationService;
        private LogOnModel logOnModel;

        protected override void AfterSetUp()
        {
            commandDispatcher = MockRepository.GenerateMock<ICommandDispatcher>();
            formsAuthenticationService = MockRepository.GenerateMock<IFormsAuthenticationService>();
            controller = new AccountController(commandDispatcher, formsAuthenticationService);
            controller.ControllerContext = new ControllerContext(HttpContext, RouteData, controller);
            logOnModel = new LogOnModel {UserName = "userX", Password = "secret", RememberMe = true};
            controller.Url = new UrlHelper(new RequestContext(HttpContext, RouteData));
            HttpRequest.Stub(r => r.Url).Return(new Uri("http://nebula.be/Account/LogOn"));
        }

        [Test]
        public void Should_add_an_error_and_return_view_if_command_returns_false()
        {
            commandDispatcher.Stub(d => d.Dispatch<LogOnUserCommand, OperationResult>(Arg<LogOnUserCommand>.Is.Anything)).Return(false);

            var result = controller.LogOn(logOnModel, "/");

            Assert.IsFalse(controller.ModelState.IsValid);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreSame(logOnModel, ((ViewResult) result).Model);
        }

        [Test]
        public void Should_add_an_error_if_the_command_throws_unknown_account_exception()
        {
            commandDispatcher.Stub(d => d.Dispatch<LogOnUserCommand, OperationResult>(Arg<LogOnUserCommand>.Is.Anything)).Throw(new UnknownAccountException());

            var result = controller.LogOn(logOnModel, "/");

            Assert.IsFalse(controller.ModelState.IsValid);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreSame(logOnModel, ((ViewResult)result).Model);
        }

        [Test]
        public void Should_dispatch_the_expected_command()
        {
            commandDispatcher.Expect(
                d =>
                d.Dispatch<LogOnUserCommand, OperationResult>(
                    Arg<LogOnUserCommand>.Matches(c => c.UserName == logOnModel.UserName && c.Password == logOnModel.Password))).Return(true);

            controller.LogOn(logOnModel, "/");

            commandDispatcher.VerifyAllExpectations();
        }

        [Test]
        public void Should_redirect_to_home_if_it_isnt_local_url_and_command_returned_true()
        {
            commandDispatcher.Stub(d => d.Dispatch<LogOnUserCommand, OperationResult>(Arg<LogOnUserCommand>.Is.Anything)).Return(true);

            var result = controller.LogOn(logOnModel, "http://www.google.be");

            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            var redirectToRouteResult = (RedirectToRouteResult) result;
            Assert.AreEqual("Home", redirectToRouteResult.RouteValues["controller"]);
            Assert.AreEqual("Index", redirectToRouteResult.RouteValues["action"]);
        }

        [Test]
        public void Should_redirect_to_return_url_if_it_is_local_and_command_returned_true()
        {
            commandDispatcher.Stub(d => d.Dispatch<LogOnUserCommand, OperationResult>(Arg<LogOnUserCommand>.Is.Anything)).Return(true);

            var result = controller.LogOn(logOnModel, "/foo/bar");

            Assert.IsInstanceOf<RedirectResult>(result);
            Assert.AreEqual("/foo/bar", ((RedirectResult) result).Url);
        }

        [Test]
        public void Should_return_view_if_model_is_invalid()
        {
            controller.ModelState.AddModelError("", "Something is wrong");

            var result = controller.LogOn(logOnModel, "/");

            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreSame(logOnModel, ((ViewResult) result).Model);
        }

        [Test]
        public void Should_sign_in_user_as_expected_if_command_is_true_with_rememberme_false()
        {
            commandDispatcher.Stub(d => d.Dispatch<LogOnUserCommand, OperationResult>(Arg<LogOnUserCommand>.Is.Anything)).Return(true);
            formsAuthenticationService.Expect(s => s.SignIn(logOnModel.UserName, false));

            logOnModel.RememberMe = false;

            controller.LogOn(logOnModel, "/");

            formsAuthenticationService.VerifyAllExpectations();
        }

        [Test]
        public void Should_sign_in_user_as_expected_if_command_returns_true()
        {
            commandDispatcher.Stub(d => d.Dispatch<LogOnUserCommand, OperationResult>(Arg<LogOnUserCommand>.Is.Anything)).Return(true);
            formsAuthenticationService.Expect(s => s.SignIn(logOnModel.UserName, logOnModel.RememberMe));

            controller.LogOn(logOnModel, "/");

            formsAuthenticationService.VerifyAllExpectations();
        }
    }
}