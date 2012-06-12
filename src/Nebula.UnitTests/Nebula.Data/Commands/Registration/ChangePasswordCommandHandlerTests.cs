﻿using NUnit.Framework;
using Nebula.Contracts.Registration;
using Nebula.Data.Commands.Registration;
using Nebula.Domain.Registration;
using Nebula.Domain.Registration.Queries;
using Rhino.Mocks;

namespace Nebula.UnitTests.Nebula.Data.Commands.Registration
{
    [TestFixture]
    public class ChangePasswordCommandHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            query = MockRepository.GenerateMock<IGetAccountByUserNameQuery>();
            commandHandler = new ChangePasswordCommandHandler(query);
            command = new ChangePasswordCommand
                          {
                              UserName = "userx",
                              OldPassword = "oldsecret",
                              NewPassword = "newsecret"
                          };
        }

        private ChangePasswordCommandHandler commandHandler;
        private IGetAccountByUserNameQuery query;
        private ChangePasswordCommand command;

        private Account BuildAccount()
        {
            return new Account
                       {
                           UserName = "userx",
                           Password = new Password("secret")
                       };
        }

        [Test]
        public void Should_invoke_the_query_as_expected()
        {
            query.Expect(q => q.Execute(command.UserName)).Return(BuildAccount());

            commandHandler.Handle(command);

            query.VerifyAllExpectations();
        }

        [Test]
        public void Should_return_false_if_the_password_does_not_match()
        {
            query.Stub(q => q.Execute(command.UserName)).Return(BuildAccount());

            var result = commandHandler.Handle(command);

            Assert.IsFalse(result);
        }

        [Test]
        public void Should_return_true_if_the_password_matches()
        {
            var account = BuildAccount();
            account.Password = new Password(command.OldPassword);

            query.Stub(q => q.Execute(command.UserName)).Return(account);

            var result = commandHandler.Handle(command);

            Assert.IsTrue(result);
        }

        [Test]
        public void Should_throw_unknown_account_exception_if_user_does_not_exist()
        {
            query.Stub(q => q.Execute(command.UserName)).Return(null);

            Assert.Throws<UnknownAccountException>(() => commandHandler.Handle(command));
        }

        [Test]
        public void Should_update_the_password()
        {
            var account = BuildAccount();
            account.Password = new Password(command.OldPassword);

            query.Stub(q => q.Execute(command.UserName)).Return(account);

            commandHandler.Handle(command);

            Assert.IsTrue(account.Password.Verify(command.NewPassword));
        }
    }
}