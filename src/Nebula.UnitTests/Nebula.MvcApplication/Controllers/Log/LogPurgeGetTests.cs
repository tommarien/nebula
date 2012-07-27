using System.Web.Mvc;
using NUnit.Framework;
using Nebula.Contracts.System.Commands;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Querying;
using Nebula.MvcApplication.Controllers;
using Rhino.Mocks;

namespace Nebula.UnitTests.Nebula.MvcApplication.Controllers.Log
{
    [TestFixture]
    public class LogPurgeGetTests : HttpContextFixture
    {
        private LogController controller;
        private ICommandDispatcher dispatcher;

        protected override void AfterSetUp()
        {
            dispatcher = MockRepository.GenerateStrictMock<ICommandDispatcher>();
            controller = new LogController(MockRepository.GenerateStrictMock<IQueryHandlerFactory>(), dispatcher);
        }

        [Test]
        public void Should_behave_as_expected()
        {
            dispatcher.Expect(d => d.Dispatch(Arg<PurgeEventLogOlderThan1WeekCommand>.Is.Anything));

            var result = controller.Purge();

            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            CollectionAssert.DoesNotContain(((RedirectToRouteResult) result).RouteValues.Keys, "controller");
            Assert.AreEqual("Index", ((RedirectToRouteResult) result).RouteValues["action"]);
        }
    }
}