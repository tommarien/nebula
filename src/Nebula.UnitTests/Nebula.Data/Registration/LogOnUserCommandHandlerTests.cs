using System;
using NUnit.Framework;
using Nebula.Contracts.Registration;
using Nebula.Contracts.Registration.Commands;
using Nebula.Contracts.Registration.Exceptions;
using Nebula.Data.Registration.Commands;
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
            queryHandler = MockRepository.GenerateMock<IQueryHandler<AccountQuery, Account>>();
            commandHandler = new LogOnUserCommandHandler(queryHandler);
            command = new LogOnUserCommand
                {
                    UserName = AccountBuilder.DefaultUserName,
                    Password = AccountBuilder.DefaultPassword
                };
        }

        private LogOnUserCommandHandler commandHandler;
        private IQueryHandler<AccountQuery, Account> queryHandler;
        private LogOnUserCommand command;

        [Test]
        public void Should_invoke_the_query_as_expected()
        {
            queryHandler.Expect(h => h.Execute(Arg<AccountQuery>.Matches(q => q.UserName == command.UserName)))
                .Return(new AccountBuilder().Build());

            commandHandler.Handle(command);

            queryHandler.VerifyAllExpectations();
        }

        [Test]
        public void Should_return_false_if_the_password_does_not_match()
        {
            queryHandler.Stub(h => h.Execute(Arg<AccountQuery>.Matches(q => q.UserName == command.UserName)))
                .Return(new AccountBuilder().WithPassword("somethingelse").Build());

            bool result = commandHandler.Handle(command);

            Assert.IsFalse(result);
        }

        [Test]
        public void Should_return_false_if_user_does_not_exist()
        {
            queryHandler.Stub(h => h.Execute(Arg<AccountQuery>.Matches(q => q.UserName == command.UserName)))
                .Return(null);

            bool result = commandHandler.Handle(command);

            Assert.IsFalse(result);
        }

        [Test]
        public void Should_return_true_if_the_password_matches()
        {
            queryHandler.Stub(h => h.Execute(Arg<AccountQuery>.Matches(q => q.UserName == command.UserName)))
                .Return(new AccountBuilder().Build());

            bool result = commandHandler.Handle(command);

            Assert.IsTrue(result);
        }

        [Test]
        public void Should_set_LastLogonDate()
        {
            var account = new AccountBuilder().Build();
            var clock = MockRepository.GenerateStub<ISystemClock>();
            SystemContext.Clock = clock;

            queryHandler.Stub(h => h.Execute(Arg<AccountQuery>.Matches(q => q.UserName == command.UserName))).Return(account);

            var aDate = new DateTime(2012, 1, 1);
            clock.Stub(c => c.Now()).Return(aDate);

            commandHandler.Handle(command);

            Assert.AreEqual(aDate, account.LastLogOnDate);
        }

        [Test]
        public void Throws_InactiveAccountException_if_the_account_has_been_deactivated()
        {
            var account = new AccountBuilder()
                .AsInactive()
                .Build();

            queryHandler.Stub(h => h.Execute(Arg<AccountQuery>.Matches(q => q.UserName == command.UserName)))
                .Return(account);

            Assert.Throws<InactiveAccountException>(() => commandHandler.Handle(command));
        }
    }
}