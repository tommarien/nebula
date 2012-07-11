using System.Web.Mvc;
using NUnit.Framework;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Querying;
using Nebula.MvcApplication.Controllers;
using Nebula.MvcApplication.Services;
using Rhino.Mocks;

namespace Nebula.UnitTests.Nebula.MvcApplication.Controllers.Account
{
    [TestFixture]
    public class AccountChangePasswordSuccessGetTests : HttpContextFixture
    {
        private AccountController controller;

        protected override void AfterSetUp()
        {
            controller = new AccountController(MockRepository.GenerateStrictMock<ICommandDispatcher>(),
                                               MockRepository.GenerateStrictMock<IQueryHandlerFactory>(),
                                               MockRepository.GenerateStrictMock<IFormsAuthenticationService>());

            SetupControllerContext(controller);
        }

        [Test]
        public void Should_return_view()
        {
            var result = controller.ChangePasswordSuccess();

            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}