using System.Collections.Generic;
using NUnit.Framework;
using Nebula.Contracts.System.Queries;
using Nebula.Infrastructure;
using Nebula.Infrastructure.Querying;
using Nebula.Infrastructure.Querying.QueryResults;
using Nebula.MvcApplication.Controllers;
using Nebula.MvcApplication.Models;
using Rhino.Mocks;

namespace Nebula.UnitTests.Nebula.MvcApplication.Controllers.System
{
    [TestFixture]
    public class SystemPagedLogSummaryGetTests : HttpContextFixture
    {
        private IQueryHandlerFactory queryHandlerFactory;
        private SystemController controller;
        private IPagedLogSummaryQueryHandler queryHandler;

        protected override void AfterSetUp()
        {
            queryHandlerFactory = MockRepository.GenerateStub<IQueryHandlerFactory>();
            queryHandler = MockRepository.GenerateStub<IPagedLogSummaryQueryHandler>();
            controller = new SystemController(queryHandlerFactory);

            queryHandlerFactory.Stub(f => f.CreateQuery<IPagedLogSummaryQueryHandler>()).Return(queryHandler);

            SetupControllerContext(controller);
        }

        [Test]
        public void Should_behave_as_expected()
        {
            var queryResults = new LogSummary[] {};
            queryHandler.Stub(qh => qh.Execute(Arg<LogSummarySearch>.Matches(ls => ls.Skip == 0 && ls.Take == 10))).Return(
                new PagedResult<LogSummary>(queryResults, 10));

            var result = controller.PagedLogSummary(new DatatableModel {sEcho = "20", iDisplayStart = 0, iDisplayLength = 10});

            Assert.IsNotNull(result.Data);
            IDictionary<string, object> values = result.Data.ToDictionary();

            Assert.AreEqual("20", values["sEcho"]);
            Assert.AreEqual(10, values["iTotalRecords"]);
            Assert.AreEqual(10, values["iTotalDisplayRecords"]);
            Assert.AreEqual(queryResults, values["aaData"]);
        }
    }
}