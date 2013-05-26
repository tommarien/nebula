using System;
using NSubstitute;
using NUnit.Framework;
using Nebula.Contracts.Registration;
using Nebula.Contracts.Registration.Commands;
using Nebula.Contracts.Registration.Exceptions;
using Nebula.Data.Registration.Commands;
using Nebula.Domain.Registration;
using Nebula.Infrastructure;
using Nebula.Infrastructure.Querying;
using Nebula.UnitTests.Builders;

namespace Nebula.UnitTests.Nebula.Data.Registration
{
    [TestFixture]
    public class LogOnUserCommandHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            queryHandler = Substitute.For<IQueryHandler<AccountQuery, Account>>();
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
            queryHandler.Execute(Arg.Any<AccountQuery>())
                        .Returns(new AccountBuilder().Build());

            commandHandler.Handle(command);

            queryHandler.Received().Execute(Arg.Is<AccountQuery>(q => q.UserName == command.UserName));
        }

        [Test]
        public void Should_not_throw_any_exceptions_if_everything_is_fine()
        {
            queryHandler.Execute(Arg.Any<AccountQuery>())
                        .Returns(new AccountBuilder().Build());

            Assert.DoesNotThrow(() => commandHandler.Handle(command));
        }

        [Test]
        public void Should_set_LastLogonDate()
        {
            Account account = new AccountBuilder().Build();
            var clock = Substitute.For<ISystemClock>();
            SystemContext.Clock = clock;

            queryHandler.Execute(Arg.Any<AccountQuery>())
                        .Returns(account);

            var aDate = new DateTime(2012, 1, 1);
            clock.Now().Returns(aDate);

            commandHandler.Handle(command);

            Assert.AreEqual(aDate, account.LastLogOnDate);
        }

        [Test]
        public void Throws_AuthenticationFailedException_if_passwords_dont_match()
        {
            queryHandler.Execute(Arg.Any<AccountQuery>())
                        .Returns(new AccountBuilder().WithPassword("somethingelse").Build());

            Assert.Throws<AuthenticationFailedException>(() => commandHandler.Handle(command));
        }

        [Test]
        public void Throws_AuthenticationFailedException_if_user_does_not_exist()
        {
            queryHandler.Execute(Arg.Any<AccountQuery>())
                        .Returns((Account) null);

            Assert.Throws<AuthenticationFailedException>(() => commandHandler.Handle(command));
        }

        [Test]
        public void Throws_InactiveAccountException_if_the_account_has_been_deactivated()
        {
            Account account = new AccountBuilder()
                .AsInactive()
                .Build();

            queryHandler.Execute(Arg.Any<AccountQuery>())
                        .Returns(account);

            Assert.Throws<InactiveAccountException>(() => commandHandler.Handle(command));
        }
    }
}