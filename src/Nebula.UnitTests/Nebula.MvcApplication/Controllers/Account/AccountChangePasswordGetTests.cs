using System.Web.Mvc;
using NUnit.Framework;
using Nebula.Infrastructure.Commanding;
using Nebula.MvcApplication.Controllers;
using Nebula.MvcApplication.Services;
using Rhino.Mocks;

namespace Nebula.UnitTests.Nebula.MvcApplication.Controllers.Account
{
    [TestFixture]
    public class AccountChangePasswordGetTests : HttpContextFixture
    {
        private AccountController controller;

        protected override void AfterSetUp()
        {
            controller = new AccountController(MockRepository.GenerateStub<ICommandDispatcher>()
                                               , MockRepository.GenerateStub<IFormsAuthenticationService>());

            controller.ControllerContext = new ControllerContext(HttpContext, RouteData, controller);
        }

        [Test]
        public void Should_only_be_available_for_authenticated_users()
        {
            var attributes = typeof (AccountController).GetMethod("ChangePassword")
                .GetCustomAttributes(typeof (AuthorizeAttribute), true);

            CollectionAssert.IsNotEmpty(attributes, "The action method should have been decorated with an AuthorizeAttribute");
        }

        [Test]
        public void Should_return_MyProfile_View()
        {
            var result = controller.ChangePassword();

            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}