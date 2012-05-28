using NUnit.Framework;
using Nebula.Domain.Registration;

namespace Nebula.IntegrationTests.Registration
{
    [TestFixture]
    public class UserAccountCrud : CrudFixture<UserAccount, int>
    {
        protected override UserAccount BuildEntity()
        {
            return new UserAccount
                       {
                           UserName = "jennajameson",
                           Password = "letmein"
                       };
        }

        protected override void ModifyEntity(UserAccount entity)
        {
            entity.UserName = "test";
            entity.Password = "secret";
        }

        protected override void AssertAreEqual(UserAccount expectedEntity, UserAccount actualEntity)
        {
            Assert.AreEqual(expectedEntity.UserName, actualEntity.UserName);
            Assert.AreEqual(expectedEntity.Password, actualEntity.Password);
        }

        protected override void AssertValidId(UserAccount entity)
        {
            Assert.Greater(entity.Id, 0);
        }
    }
}