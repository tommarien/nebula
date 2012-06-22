using System.Web.Mvc;
using NUnit.Framework;
using Nebula.Contracts.Registration;
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
                                               , MockRepository.GenerateStub<IFormsAuthenticationService>(),
                                               MockRepository.GenerateStub<IGetRolesForUserQuery>());

            controller.ControllerContext = new ControllerContext(HttpContext, RouteData, controller);
        }

        [Test]
        public void Should_return_viewResult()
        {
            var result = controller.ChangePassword();

            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}