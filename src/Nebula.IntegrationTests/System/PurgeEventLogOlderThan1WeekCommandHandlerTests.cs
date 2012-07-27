using System;
using System.Collections.Generic;
using NUnit.Framework;
using Nebula.Contracts.System.Commands;
using Nebula.Data.System.Commands;
using Nebula.Domain.System;
using Nebula.Infrastructure;
using Rhino.Mocks;

namespace Nebula.IntegrationTests.System
{
    [TestFixture]
    public class PurgeEventLogOlderThan1WeekCommandHandlerTests : AutoRollbackFixture
    {
        private List<Log> population;
        private DateTime LastUsedDate;
        private PurgeEventLogOlderThan1WeekCommandHandler commandHandler;

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

            commandHandler = new PurgeEventLogOlderThan1WeekCommandHandler(Session);
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

            LastUsedDate = LastUsedDate.AddDays(1);

            return log;
        }

        [Test]
        public void ShouldBehaveAsExpected()
        {
            SystemContext.Clock = MockRepository.GenerateStub<ISystemClock>();
            SystemContext.Clock.Stub(c => c.Today()).Return(new DateTime(2012, 1, 10));

            commandHandler.Handle(new PurgeEventLogOlderThan1WeekCommand());
            var result = Session.QueryOver<Log>().RowCount();
            Assert.AreEqual(8, result);
        }
    }
}