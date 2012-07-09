using System;
using NUnit.Framework;
using Nebula.Contracts.System.Queries;
using Nebula.Data.System.Queries;
using Nebula.Domain.System;

namespace Nebula.IntegrationTests.System
{
    [TestFixture]
    public class LogEntryDetailsQueryHandlerTests : AutoRollbackFixture
    {
        private LogEntryDetailsQueryHandler handler;
        private Log addedLog;

        protected override void AfterSetUp()
        {
            addedLog = new Log
                           {
                               Date = new DateTime(2012, 1, 1),
                               Exception = "Exception",
                               HostName = "HostName",
                               Level = "ERROR",
                               Logger = "Logger",
                               Message = "Message",
                               SessionId = "SessionId",
                               UserName = "UserName"
                           };

            Session.Save(addedLog);

            FlushAndClear();

            handler = new LogEntryDetailsQueryHandler(Session);
        }

        private void AssertAreEqual(Log log, LogEntry logEntry)
        {
            Assert.AreEqual(log.Id, logEntry.Id);
            Assert.AreEqual(log.Date, logEntry.Date);
            Assert.AreEqual(log.Level, logEntry.Level);
            Assert.AreEqual(log.Logger, logEntry.Logger);
            Assert.AreEqual(log.Message, logEntry.Message);
            Assert.AreEqual(log.Exception, logEntry.Exception);
            Assert.AreEqual(log.SessionId, logEntry.SessionId);
            Assert.AreEqual(log.HostName, logEntry.HostName);
            Assert.AreEqual(log.UserName, logEntry.UserName);
        }

        [Test]
        public void Should_return_expected_result()
        {
            var entry = handler.Execute(addedLog.Id);
            AssertAreEqual(addedLog, entry);
        }

        [Test]
        public void Should_return_null_if_log_does_not_exist()
        {
            var entry = handler.Execute(0);

            Assert.IsNull(entry);
        }
    }
}