using NUnit.Framework;
using Nebula.Contracts.Registration;
using Nebula.Contracts.Registration.Commands;
using Nebula.Contracts.Registration.Exceptions;
using Nebula.Data.Registration.Commands;
using Nebula.Domain.Registration;
using Nebula.Infrastructure.Commanding.CommandResults;
using Nebula.Infrastructure.Querying;
using Nebula.UnitTests.Builders;
using Rhino.Mocks;

namespace Nebula.UnitTests.Nebula.Data.Registration
{
    [TestFixture]
    public class ChangePasswordCommandHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            queryHandler = MockRepository.GenerateMock<IQueryHandler<AccountQuery, Account>>();
            commandHandler = new ChangePasswordCommandHandler(queryHandler);
            command = new ChangePasswordCommand
                {
                    UserName = "userx",
                    OldPassword = "oldsecret",
                    NewPassword = "newsecret"
                };
        }

        private ChangePasswordCommandHandler commandHandler;
        private IQueryHandler<AccountQuery, Account> queryHandler;
        private ChangePasswordCommand command;

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
                .Return(new AccountBuilder().Build());

            OperationResult result = commandHandler.Handle(command);

            Assert.IsFalse(result);
        }

        [Test]
        public void Should_return_true_if_the_password_matches()
        {
            Account account = new AccountBuilder()
                .WithPassword(command.OldPassword)
                .Build();

            queryHandler.Stub(h => h.Execute(Arg<AccountQuery>.Matches(q => q.UserName == command.UserName)))
                .Return(account);

            OperationResult result = commandHandler.Handle(command);

            Assert.IsTrue(result);
        }

        [Test]
        public void Should_throw_unknown_account_exception_if_user_does_not_exist()
        {
            queryHandler.Stub(h => h.Execute(Arg<AccountQuery>.Matches(q => q.UserName == command.UserName)))
                .Return(null);

            Assert.Throws<UnknownAccountException>(() => commandHandler.Handle(command));
        }

        [Test]
        public void Should_update_the_password()
        {
            Account account = new AccountBuilder()
                .WithPassword(command.OldPassword)
                .Build();

            queryHandler.Stub(h => h.Execute(Arg<AccountQuery>.Matches(q => q.UserName == command.UserName)))
                .Return(account);

            commandHandler.Handle(command);

            Assert.IsTrue(account.Password.Equals(command.NewPassword));
        }
    }
}