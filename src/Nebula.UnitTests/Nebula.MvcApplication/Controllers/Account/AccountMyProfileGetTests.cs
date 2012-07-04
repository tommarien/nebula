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
    public class AccountMyProfileGetTests : HttpContextFixture
    {
        private AccountController controller;

        protected override void AfterSetUp()
        {
            controller = new AccountController(MockRepository.GenerateStub<ICommandDispatcher>()
                                               , MockRepository.GenerateStub<IFormsAuthenticationService>()
                                               , MockRepository.GenerateStub<IGetAccountRolesQueryHandler>());

            controller.ControllerContext = new ControllerContext(HttpContext, RouteData, controller);
        }

        [Test]
        public void Should_return_viewResult()
        {
            var result = controller.MyProfile();

            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}