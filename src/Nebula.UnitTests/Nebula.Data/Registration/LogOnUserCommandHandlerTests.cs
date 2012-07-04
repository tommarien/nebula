using System;
using NUnit.Framework;
using Nebula.Contracts.Registration.Commands;
using Nebula.Contracts.Registration.Exceptions;
using Nebula.Data.Commands.Registration;
using Nebula.Domain.Registration;
using Nebula.Infrastructure;
using Nebula.Infrastructure.Querying;
using Nebula.UnitTests.Builders;
using Rhino.Mocks;

namespace Nebula.UnitTests.Nebula.Data.Registration
{
    [TestFixture]
    public class LogOnUserCommandHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            query = MockRepository.GenerateMock<IQueryHandler<string, Account>>();
            commandHandler = new LogOnUserCommandHandler(query);
            command = new LogOnUserCommand
                          {
                              UserName = AccountBuilder.DefaultUserName,
                              Password = AccountBuilder.DefaultPassword
                          };
        }

        private LogOnUserCommandHandler commandHandler;
        private IQueryHandler<string, Account> query;
        private LogOnUserCommand command;

        [Test]
        public void Should_invoke_the_query_as_expected()
        {
            query.Expect(q => q.Execute(command.UserName)).Return(new AccountBuilder().Build());

            commandHandler.Handle(command);

            query.VerifyAllExpectations();
        }

        [Test]
        public void Should_return_false_if_the_password_does_not_match()
        {
            query.Stub(q => q.Execute(command.UserName)).Return(new AccountBuilder().WithPassword("somethingelse").Build());

            bool result = commandHandler.Handle(command);

            Assert.IsFalse(result);
        }

        [Test]
        public void Should_return_false_if_user_does_not_exist()
        {
            query.Stub(q => q.Execute(command.UserName)).Return(null);

            bool result = commandHandler.Handle(command);

            Assert.IsFalse(result);
        }

        [Test]
        public void Should_return_true_if_the_password_matches()
        {
            query.Stub(q => q.Execute(command.UserName)).Return(new AccountBuilder().Build());

            bool result = commandHandler.Handle(command);

            Assert.IsTrue(result);
        }

        [Test]
        public void Should_set_LastLogonDate()
        {
            var account = new AccountBuilder().Build();

            query.Stub(q => q.Execute(command.UserName)).Return(account);

            var aDate = new DateTime(2012, 1, 1);
            SystemClock.Mock(aDate);

            commandHandler.Handle(command);

            Assert.AreEqual(aDate, account.LastLogOnDate);
        }

        [Test]
        public void Throws_InactiveAccountException_if_the_account_has_been_deactivated()
        {
            var account = new AccountBuilder()
                .AsInactive()
                .Build();

            query.Stub(q => q.Execute(command.UserName)).Return(account);

            Assert.Throws<InactiveAccountException>(() => commandHandler.Handle(command));
        }
    }
}