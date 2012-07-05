using System.Collections.Generic;
using NUnit.Framework;
using Nebula.Contracts.System.Queries;
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

        private IDictionary<string, object> ToDictionary(object o)
        {
            // TODO : Find a better way, maybe existing way to handle this !
            var dict = new Dictionary<string, object>();
            if (o == null) return dict;

            foreach (var propertyInfo in o.GetType().GetProperties())
                dict.Add(propertyInfo.Name, propertyInfo.GetValue(o, null));

            return dict;
        }

        [Test]
        public void Should_behave_as_expected()
        {
            var queryResults = new LogSummary[] {};
            queryHandler.Stub(qh => qh.Execute(Arg<LogSummarySearch>.Matches(ls => ls.Skip == 0 && ls.Take == 10))).Return(
                new PagedResult<LogSummary>(queryResults, 10));

            var result = controller.PagedLogSummary(new DatatableModel {sEcho = "20", iDisplayStart = 0, iDisplayLength = 10});

            var data = ToDictionary(result.Data);

            Assert.AreEqual("20", data["sEcho"]);
            Assert.AreEqual(10, data["iTotalRecords"]);
            Assert.AreEqual(10, data["iTotalDisplayRecords"]);
            Assert.AreEqual(queryResults, data["aaData"]);
        }
    }
}