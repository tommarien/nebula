using NUnit.Framework;
using Nebula.Contracts.Registration;
using Nebula.Data.Registration.Queries;
using Nebula.Domain.Registration;
using Nebula.Infrastructure.Querying;
using Rhino.Mocks;

namespace Nebula.UnitTests.Nebula.Data.Registration
{
    [TestFixture]
    public class GetAccountRolesQueryHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            getAccountQueryHandler = MockRepository.GenerateMock<IQuery<string, Account>>();
            getAccountRolesQueryHandler = new GetAccountRolesQueryHandler(getAccountQueryHandler);
        }

        private IQuery<string, Account> getAccountQueryHandler;
        private GetAccountRolesQueryHandler getAccountRolesQueryHandler;

        [Test]
        public void Should_invoke_the_underlying_query_as_expected()
        {
            var account = Accounts.Administrator.Build();

            getAccountQueryHandler.Expect(q => q.Execute(account.UserName)).Return(account);

            getAccountRolesQueryHandler.Execute(account.UserName);

            getAccountQueryHandler.VerifyAllExpectations();
        }

        [Test]
        public void Should_return_empty_array_if_account_is_not_existing()
        {
            getAccountQueryHandler.Stub(q => q.Execute(Arg<string>.Is.Anything)).Return(null);

            var result = getAccountRolesQueryHandler.Execute("youhoe");

            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void Should_return_expected_roles()
        {
            var account = Accounts.Administrator.Build();

            getAccountQueryHandler.Stub(q => q.Execute(Arg<string>.Is.Anything)).Return(account);

            var roles = getAccountRolesQueryHandler.Execute(account.UserName);

            CollectionAssert.Contains(roles, Role.Administrator);
        }
    }
}