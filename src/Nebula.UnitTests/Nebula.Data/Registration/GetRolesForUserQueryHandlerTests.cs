using NUnit.Framework;
using Nebula.Contracts.Registration;
using Nebula.Data.Queries.Registration;
using Nebula.Domain.Registration.Queries;
using Rhino.Mocks;

namespace Nebula.UnitTests.Nebula.Data.Registration
{
    [TestFixture]
    public class GetRolesForUserQueryHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            query = MockRepository.GenerateMock<IGetAccountByUserNameQuery>();
            queryHandler = new GetRolesForUserQueryHandler(query);
        }

        private IGetAccountByUserNameQuery query;
        private GetRolesForUserQueryHandler queryHandler;

        [Test]
        public void Should_invoke_the_underlying_query_as_expected()
        {
            var account = Accounts.Administrator.Build();

            query.Expect(q => q.Execute(account.UserName)).Return(account);

            queryHandler.Execute(account.UserName);

            query.VerifyAllExpectations();
        }

        [Test]
        public void Should_return_empty_array_if_account_is_not_existing()
        {
            query.Stub(q => q.Execute(Arg<string>.Is.Anything)).Return(null);

            var result = queryHandler.Execute("youhoe");

            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void Should_return_expected_roles()
        {
            var account = Accounts.Administrator.Build();

            query.Stub(q => q.Execute(Arg<string>.Is.Anything)).Return(account);

            var roles = queryHandler.Execute(account.UserName);

            CollectionAssert.Contains(roles, Role.Administrator);
        }
    }
}