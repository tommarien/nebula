using NUnit.Framework;
using Nebula.Domain.Registration;

namespace Nebula.IntegrationTests.Registration
{
    [TestFixture]
    public class AccountCrud : CrudFixture<Account, int>
    {
        protected override Account BuildEntity()
        {
            return new Account
                       {
                           UserName = "jennajameson",
                           Password = "letmein"
                       };
        }

        protected override void ModifyEntity(Account entity)
        {
            entity.UserName = "test";
            entity.Password = "secret";
        }

        protected override void AssertAreEqual(Account expectedEntity, Account actualEntity)
        {
            Assert.AreEqual(expectedEntity.UserName, actualEntity.UserName);
            Assert.AreEqual(expectedEntity.Password, actualEntity.Password);
        }

        protected override void AssertValidId(Account entity)
        {
            Assert.Greater(entity.Id, 0);
        }
    }
}