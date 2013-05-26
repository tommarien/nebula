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
    public class ChangePasswordCommandHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            queryHandler = Substitute.For<IQueryHandler<AccountQuery, Account>>();
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
            queryHandler.Execute(Arg.Any<AccountQuery>())
                        .Returns(new AccountBuilder().WithPassword(command.OldPassword).Build());

            commandHandler.Handle(command);

            queryHandler.Received().Execute(Arg.Is<AccountQuery>(query => query.UserName == command.UserName));
        }

        [Test]
        public void Should_throw_unknown_account_exception_if_user_does_not_exist()
        {
            queryHandler.Execute(Arg.Any<AccountQuery>())
                        .Returns((Account) null);

            Assert.Throws<UnknownAccountException>(() => commandHandler.Handle(command));
        }

        [Test]
        public void Should_update_the_password()
        {
            Account account = new AccountBuilder()
                .WithPassword(command.OldPassword)
                .Build();

            queryHandler.Execute(Arg.Any<AccountQuery>())
                        .Returns(account);

            commandHandler.Handle(command);

            Assert.IsTrue(account.Password.Equals(command.NewPassword));
        }

        [Test]
        public void Throws_business_exception_if_the_passwords_did_not_match()
        {
            queryHandler.Execute(Arg.Any<AccountQuery>())
                        .Returns(new AccountBuilder().Build());

            Assert.Throws<BusinessException>(() => commandHandler.Handle(command));
        }
    }
}