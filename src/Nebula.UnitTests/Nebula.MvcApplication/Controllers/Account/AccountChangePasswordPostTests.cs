using System;
using System.Security.Principal;
using System.Web.Mvc;
using NUnit.Framework;
using Nebula.Contracts.Registration.Commands;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Querying;
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
        private ICommandBus commandBus;
        private IFormsAuthenticationService formsAuthenticationService;
        private ChangePasswordModel model;

        protected override void AfterSetUp()
        {
            commandBus = MockRepository.GenerateMock<ICommandBus>();
            formsAuthenticationService = MockRepository.GenerateMock<IFormsAuthenticationService>();

            controller = new AccountController(commandBus, MockRepository.GenerateStrictMock<IQueryHandlerFactory>(),
                                               formsAuthenticationService);
            SetupControllerContext(controller);
            model = new ChangePasswordModel
                {
                    OldPassword = "oldsecret",
                    NewPassword = "newsecret",
                    ConfirmPassword = "newsecret"
                };
            HttpContext.Stub(c => c.User).Return(new GenericPrincipal(new GenericIdentity("userX"), new string[] {}));
        }

        [Test]
        public void Should_add_an_error_and_return_view_anything_goes_wrong()
        {
            commandBus.Stub(bus => bus.Send(Arg<ChangePasswordCommand>.Is.Anything))
                      .Throw(new Exception());

            ActionResult result = controller.ChangePassword(model);

            Assert.IsFalse(controller.ModelState.IsValid);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreSame(model, ((ViewResult) result).Model);
        }

        [Test]
        public void Should_return_ChangePasswordSuccess_if_succeeds()
        {
            commandBus.Stub(
                d => d.Send(Arg<ChangePasswordCommand>.Is.Anything));

            ActionResult result = controller.ChangePassword(model);

            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.AreEqual("ChangePasswordSuccess", ((RedirectToRouteResult) result).RouteValues["action"]);
            Assert.IsNull(((RedirectToRouteResult) result).RouteValues["controller"]);
        }

        [Test]
        public void Should_return_view_if_model_is_invalid()
        {
            controller.ModelState.AddModelError("", "Something is wrong");

            ActionResult result = controller.ChangePassword(model);

            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreSame(model, ((ViewResult) result).Model);
        }

        [Test]
        public void Should_send_the_expected_command()
        {
            commandBus.Expect(d => d.Send(
                Arg<ChangePasswordCommand>.Matches(c => c.UserName == "userX"
                                                        && c.OldPassword == model.OldPassword
                                                        && c.NewPassword == model.NewPassword)));


            controller.ChangePassword(model);

            commandBus.VerifyAllExpectations();
        }
    }
}