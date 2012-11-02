using System;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using Nebula.Contracts.Registration;
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
        private IQueryHandler<AccountQuery, Role[]> accountRolesQueryHandler;
        private LogOnModel logOnModel;

        protected override void AfterSetUp()
        {
            commandBus = MockRepository.GenerateMock<ICommandBus>();
            formsAuthenticationService = MockRepository.GenerateMock<IFormsAuthenticationService>();
            accountRolesQueryHandler = MockRepository.GenerateMock<IQueryHandler<AccountQuery, Role[]>>();
            queryHandlerFactory = MockRepository.GenerateStub<IQueryHandlerFactory>();
            queryHandlerFactory.Stub(f => f.CreateHandler<AccountQuery, Role[]>()).Return(accountRolesQueryHandler);

            controller = new AccountController(commandBus, queryHandlerFactory, formsAuthenticationService);
            SetupControllerContext(controller);
            logOnModel = new LogOnModel {UserName = "userX", Password = "secret", RememberMe = true};
            controller.Url = new UrlHelper(new RequestContext(HttpContext, RouteData));
            HttpRequest.Stub(r => r.Url).Return(new Uri("http://nebula.be/Account/LogOn"));
        }

        [Test]
        public void Should_add_an_error_and_return_view_if_command_returns_false()
        {
            commandBus.Stub(bus => bus.SendAndReply<LogOnUserCommand, OperationResult>(Arg<LogOnUserCommand>.Is.Anything)).Return(false);

            var result = controller.LogOn(logOnModel, "/");

            Assert.IsFalse(controller.ModelState.IsValid);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreSame(logOnModel, ((ViewResult) result).Model);
        }

        [Test]
        public void Should_add_an_error_and_return_view_if_command_throws_inactiveAccountException()
        {
            commandBus.Stub(bus => bus.SendAndReply<LogOnUserCommand, OperationResult>(Arg<LogOnUserCommand>.Is.Anything)).Throw(new InactiveAccountException());

            var result = controller.LogOn(logOnModel, "/");

            Assert.IsFalse(controller.ModelState.IsValid);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreSame(logOnModel, ((ViewResult) result).Model);
        }

        [Test]
        public void Should_dispatch_the_expected_command()
        {
            commandBus.Expect(
                bus =>
                bus.SendAndReply<LogOnUserCommand, OperationResult>(
                    Arg<LogOnUserCommand>.Matches(c => c.UserName == logOnModel.UserName && c.Password == logOnModel.Password))).Return(true);

            controller.LogOn(logOnModel, "/");

            commandBus.VerifyAllExpectations();
        }

        [Test]
        public void Should_fetch_all_roles_for_user_if_command_returns_true()
        {
            commandBus.Stub(bus => bus.SendAndReply<LogOnUserCommand, OperationResult>(Arg<LogOnUserCommand>.Is.Anything)).Return(true);
            var roles = new[] {Role.Administrator};
            accountRolesQueryHandler.Expect(q => q.Execute(Arg<AccountQuery>.Matches(h => h.UserName == logOnModel.UserName)))
                .Return(roles);
            formsAuthenticationService.Stub(s => s.SignIn(logOnModel.UserName, logOnModel.RememberMe, roles));

            controller.LogOn(logOnModel, "/");

            accountRolesQueryHandler.VerifyAllExpectations();
        }

        [Test]
        public void Should_redirect_to_home_if_it_isnt_local_url_and_command_returned_true()
        {
            commandBus.Stub(d => d.SendAndReply<LogOnUserCommand, OperationResult>(Arg<LogOnUserCommand>.Is.Anything)).Return(true);

            var result = controller.LogOn(logOnModel, "http://www.google.be");

            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            var redirectToRouteResult = (RedirectToRouteResult) result;
            Assert.AreEqual("Home", redirectToRouteResult.RouteValues["controller"]);
            Assert.AreEqual("Index", redirectToRouteResult.RouteValues["action"]);
        }

        [Test]
        public void Should_redirect_to_return_url_if_it_is_local_and_command_returned_true()
        {
            commandBus.Stub(d => d.SendAndReply<LogOnUserCommand, OperationResult>(Arg<LogOnUserCommand>.Is.Anything)).Return(true);

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
            commandBus.Stub(bus => bus.SendAndReply<LogOnUserCommand, OperationResult>(Arg<LogOnUserCommand>.Is.Anything)).Return(true);
            var roles = new[] {Role.Administrator};
            accountRolesQueryHandler.Stub(q => q.Execute(Arg<AccountQuery>.Is.Anything)).Return(roles);
            formsAuthenticationService.Expect(s => s.SignIn(logOnModel.UserName, false, roles));

            logOnModel.RememberMe = false;

            controller.LogOn(logOnModel, "/");

            formsAuthenticationService.VerifyAllExpectations();
        }

        [Test]
        public void Should_sign_in_user_as_expected_if_command_returns_true()
        {
            commandBus.Stub(bus => bus.SendAndReply<LogOnUserCommand, OperationResult>(Arg<LogOnUserCommand>.Is.Anything)).Return(true);
            var roles = new[] {Role.Administrator};
            accountRolesQueryHandler.Stub(q => q.Execute(Arg<AccountQuery>.Is.Anything)).Return(roles);
            formsAuthenticationService.Expect(s => s.SignIn(logOnModel.UserName, logOnModel.RememberMe, roles));

            controller.LogOn(logOnModel, "/");

            formsAuthenticationService.VerifyAllExpectations();
        }
    }
}