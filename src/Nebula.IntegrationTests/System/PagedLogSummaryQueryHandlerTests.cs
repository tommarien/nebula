using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Nebula.Contracts.System.Queries;
using Nebula.Data.System.Queries;
using Nebula.Domain.System;

namespace Nebula.IntegrationTests.System
{
    [TestFixture]
    public class PagedLogSummaryQueryHandlerTests : AutoRollbackFixture
    {
        private PagedLogSummaryQueryHandler queryHandler;
        private List<Log> population;
        private DateTime LastUsedDate;

        protected override void AfterSetUp()
        {
            LastUsedDate = new DateTime(2012, 1, 1);
            population = new List<Log>();

            for (int i = 0; i < 10; i++)
            {
                var log = BuildLog(i);
                Session.Save(log);
                population.Add(log);
            }

            FlushAndClear();

            queryHandler = new PagedLogSummaryQueryHandler(Session);
        }

        private Log BuildLog(int index)
        {
            var log = new Log
                          {
                              Date = LastUsedDate,
                              Level = string.Format("ERROR{0}", index),
                              Logger = string.Format("Logger {0}", index),
                              Message = string.Format("Message {0}", index),
                              Exception = string.Format("Exception {0}", index),
                              HostName = string.Format("HostName {0}", index),
                              SessionId = string.Format("SessionId {0}", index),
                              UserName = string.Format("UserName {0}", index)
                          };

            LastUsedDate = LastUsedDate.AddHours(index);

            return log;
        }

        private void AssertAreEqual(Log log, LogSummary logSummary)
        {
            Assert.AreEqual(log.Id, logSummary.Id);
            Assert.AreEqual(log.Date, logSummary.Date);
            Assert.AreEqual(log.Level, logSummary.Level);
            Assert.AreEqual(log.Logger, logSummary.Logger);
            Assert.AreEqual(log.Message, logSummary.Message);
        }

        [Test]
        public void Should_return_the_expected_results()
        {
            var search = new PagedLogSummaryQuery {Skip = 5, Take = 5};
            var expectedResults = population.OrderByDescending(l => l.Id).Skip(5).Take(5).ToArray();

            var result = queryHandler.Execute(search);

            Assert.AreEqual(population.Count, result.TotalResults);
            Assert.AreEqual(5, result.Results.Count());

            for (int i = 0; i < 5; i++)
            {
                AssertAreEqual(expectedResults[i], result.Results[i]);
            }
        }
    }
}