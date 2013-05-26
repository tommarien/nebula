using System;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using Nebula.Contracts.Registration.Commands;
using Nebula.Contracts.Registration.Exceptions;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Commanding.CommandResults;
using Nebula.Infrastructure.Querying;
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
        private ICommandBus commandBus;
        private IQueryHandlerFactory queryHandlerFactory;
        private IFormsAuthenticationService formsAuthenticationService;
        private LogOnModel logOnModel;

        protected override void AfterSetUp()
        {
            commandBus = MockRepository.GenerateMock<ICommandBus>();
            formsAuthenticationService = MockRepository.GenerateMock<IFormsAuthenticationService>();
            queryHandlerFactory = MockRepository.GenerateStub<IQueryHandlerFactory>();

            controller = new AccountController(commandBus, queryHandlerFactory, formsAuthenticationService);
            SetupControllerContext(controller);
            logOnModel = new LogOnModel {UserName = "userX", Password = "secret", RememberMe = true};
            controller.Url = new UrlHelper(new RequestContext(HttpContext, RouteData));
            HttpRequest.Stub(r => r.Url).Return(new Uri("http://nebula.be/Account/LogOn"));
        }

        [Test]
        public void Should_add_an_error_and_return_view_if_command_throws_authentication_failed_exception()
        {
            commandBus.Stub(
                bus => bus.Send(Arg<LogOnUserCommand>.Is.Anything))
                      .Throw(new AuthenticationFailedException());

            ActionResult result = controller.LogOn(logOnModel, "/");

            Assert.IsFalse(controller.ModelState.IsValid);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreSame(logOnModel, ((ViewResult) result).Model);
        }

        [Test]
        public void Should_add_an_error_and_return_view_if_command_throws_inactiveAccountException()
        {
            commandBus.Stub(
                bus => bus.Send(Arg<LogOnUserCommand>.Is.Anything))
                      .Throw(new InactiveAccountException());

            ActionResult result = controller.LogOn(logOnModel, "/");

            Assert.IsFalse(controller.ModelState.IsValid);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreSame(logOnModel, ((ViewResult) result).Model);
        }

        [Test]
        public void Should_dispatch_the_expected_command()
        {
            commandBus.Expect(
                bus =>
                bus.Send(
                    Arg<LogOnUserCommand>.Matches(
                        c => c.UserName == logOnModel.UserName && c.Password == logOnModel.Password)));

            controller.LogOn(logOnModel, "/");

            commandBus.VerifyAllExpectations();
        }

        [Test]
        public void Should_redirect_to_home_if_it_isnt_local_url_and_command_is_handled()
        {
            ActionResult result = controller.LogOn(logOnModel, "http://www.google.be");

            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            var redirectToRouteResult = (RedirectToRouteResult) result;
            Assert.AreEqual("Home", redirectToRouteResult.RouteValues["controller"]);
            Assert.AreEqual("Index", redirectToRouteResult.RouteValues["action"]);
        }

        [Test]
        public void Should_redirect_to_return_url_if_it_is_local_and_command_returned_true()
        {
            commandBus.Stub(d => d.SendAndReply<LogOnUserCommand, OperationResult>(Arg<LogOnUserCommand>.Is.Anything))
                      .Return(true);

            ActionResult result = controller.LogOn(logOnModel, "/foo/bar");

            Assert.IsInstanceOf<RedirectResult>(result);
            Assert.AreEqual("/foo/bar", ((RedirectResult) result).Url);
        }

        [Test]
        public void Should_return_view_if_model_is_invalid()
        {
            controller.ModelState.AddModelError("", "Something is wrong");

            ActionResult result = controller.LogOn(logOnModel, "/");

            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreSame(logOnModel, ((ViewResult) result).Model);
        }

        [Test]
        public void Should_sign_in_user_as_expected_if_command_is_handled()
        {
            commandBus.Stub(bus => bus.Send(Arg<LogOnUserCommand>.Is.Anything));

            formsAuthenticationService.Expect(s => s.SignIn(logOnModel.UserName, logOnModel.RememberMe));

            controller.LogOn(logOnModel, "/");

            formsAuthenticationService.VerifyAllExpectations();
        }

        [Test]
        public void Should_sign_in_user_as_expected_if_command_is_handled_with_rememberme_false()
        {
            commandBus.Stub(bus => bus.Send(Arg<LogOnUserCommand>.Is.Anything));

            formsAuthenticationService.Expect(s => s.SignIn(logOnModel.UserName, false));

            logOnModel.RememberMe = false;

            controller.LogOn(logOnModel, "/");

            formsAuthenticationService.VerifyAllExpectations();
        }
    }
}