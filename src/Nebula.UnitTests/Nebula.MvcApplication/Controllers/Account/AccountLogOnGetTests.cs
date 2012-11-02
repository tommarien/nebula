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
    public class AccountLogOnGetTests : HttpContextFixture
    {
        private AccountController controller;

        protected override void AfterSetUp()
        {
            controller = new AccountController(MockRepository.GenerateStrictMock<ICommandBus>(),
                                               MockRepository.GenerateStrictMock<IQueryHandlerFactory>(),
                                               MockRepository.GenerateStrictMock<IFormsAuthenticationService>());

            SetupControllerContext(controller);
        }

        [Test]
        public void Should_return_viewResult()
        {
            var result = controller.LogOn();

            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}