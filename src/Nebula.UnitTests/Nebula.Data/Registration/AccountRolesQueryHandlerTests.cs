using NUnit.Framework;
using Nebula.Contracts.Registration;
using Nebula.Data.Registration.Queries;
using Nebula.Domain.Registration;
using Nebula.Infrastructure.Querying;
using Rhino.Mocks;

namespace Nebula.UnitTests.Nebula.Data.Registration
{
    [TestFixture]
    public class AccountRolesQueryHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            accountQueryHandler = MockRepository.GenerateMock<IQueryHandler<AccountQuery, Account>>();
            accountRolesQueryHandler = new AccountRolesQueryHandler(accountQueryHandler);
        }

        private IQueryHandler<AccountQuery, Account> accountQueryHandler;
        private AccountRolesQueryHandler accountRolesQueryHandler;

        [Test]
        public void Should_invoke_the_underlying_query_as_expected()
        {
            var account = Accounts.Administrator.Build();

            accountQueryHandler.Expect(h => h.Execute(Arg<AccountQuery>.Matches(q => q.UserName == account.UserName)))
                .Return(account);

            accountRolesQueryHandler.Execute(new AccountQuery { UserName = account.UserName });

            accountQueryHandler.VerifyAllExpectations();
        }

        [Test]
        public void Should_return_empty_array_if_account_is_not_existing()
        {
            accountQueryHandler.Stub(q => q.Execute(Arg<AccountQuery>.Is.Anything)).Return(null);

            var result = accountRolesQueryHandler.Execute(new AccountQuery { UserName = "yoehoe" });

            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void Should_return_expected_roles()
        {
            var account = Accounts.Administrator.Build();

            accountQueryHandler.Stub(h => h.Execute(Arg<AccountQuery>.Matches(q => q.UserName == account.UserName)))
                .Return(account);

            var roles = accountRolesQueryHandler.Execute(new AccountQuery { UserName = account.UserName });

            CollectionAssert.Contains(roles, Role.Administrator);
        }
    }
}