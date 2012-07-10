using NUnit.Framework;
using Nebula.Contracts.System.Queries;
using Nebula.Infrastructure.Querying;
using Nebula.MvcApplication.Controllers;
using Rhino.Mocks;

namespace Nebula.UnitTests.Nebula.MvcApplication.Controllers.System
{
    [TestFixture]
    public class SystemLogDetailGetTests : HttpContextFixture
    {
        private SystemController controller;
        private IQueryHandlerFactory queryHandlerFactory;
        private ILogEntryDetailsQueryHandler logEntryDetailsQueryHandler;

        protected override void AfterSetUp()
        {
            queryHandlerFactory = MockRepository.GenerateStub<IQueryHandlerFactory>();
            logEntryDetailsQueryHandler = MockRepository.GenerateMock<ILogEntryDetailsQueryHandler>();

            queryHandlerFactory.Stub(hf => hf.CreateQuery<ILogEntryDetailsQueryHandler>()).Return(logEntryDetailsQueryHandler);

            controller = new SystemController(queryHandlerFactory);
            SetupControllerContext(controller);
        }

        [Test]
        public void Should_behave_as_expected()
        {
            var logentry = new LogEntry();

            logEntryDetailsQueryHandler.Expect(q => q.Execute(5)).Return(logentry);

            var result = controller.LogDetail(5);

            Assert.AreSame(logentry, result.Model);
            logEntryDetailsQueryHandler.VerifyAllExpectations();
        }
    }
}