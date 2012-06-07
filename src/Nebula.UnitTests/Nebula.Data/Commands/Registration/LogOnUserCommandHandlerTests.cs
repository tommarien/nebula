using System;
using NUnit.Framework;
using Nebula.Contracts.Registration;
using Nebula.Data.Commands.Registration;
using Nebula.Domain.Registration;
using Nebula.Domain.Registration.Queries;
using Nebula.Infrastructure;
using Rhino.Mocks;

namespace Nebula.UnitTests.Nebula.Data.Commands.Registration
{
    [TestFixture]
    public class LogOnUserCommandHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            query = MockRepository.GenerateMock<IGetAccountByUserNameQuery>();
            commandHandler = new LogOnUserCommandHandler(query);
            command = new LogOnUserCommand
                          {
                              UserName = "userx",
                              Password = "secret"
                          };
        }

        private LogOnUserCommandHandler commandHandler;
        private IGetAccountByUserNameQuery query;
        private LogOnUserCommand command;

        [Test]
        public void Should_invoke_the_query_as_expected()
        {
            query.Expect(q => q.Execute(command.UserName)).Return(new Account {Password = new Password("anothersecret")});

            commandHandler.Handle(command);

            query.VerifyAllExpectations();
        }

        [Test]
        public void Should_return_false_if_the_password_does_not_match()
        {
            query.Stub(q => q.Execute(command.UserName)).Return(new Account {Password = new Password("anothersecret")});

            bool result = commandHandler.Handle(command);

            Assert.IsFalse(result);
        }

        [Test]
        public void Should_return_true_if_the_password_matches()
        {
            query.Stub(q => q.Execute(command.UserName)).Return(new Account {Password = new Password(command.Password)});

            bool result = commandHandler.Handle(command);

            Assert.IsTrue(result);
        }

        [Test]
        public void Should_set_LastLogonDate()
        {
            var account = new Account {Password = new Password(command.Password)};
            query.Stub(q => q.Execute(command.UserName)).Return(account);

            var aDate = new DateTime(2012, 1, 1);
            SystemClock.Mock(aDate);

            commandHandler.Handle(command);

            Assert.AreEqual(aDate, account.LastLogOnDate);
        }

        [Test]
        public void Should_throw_unknown_account_exception_if_user_does_not_exist()
        {
            query.Stub(q => q.Execute(command.UserName)).Return(null);

            Assert.Throws<UnknownAccountException>(() => commandHandler.Handle(command));
        }
    }
}