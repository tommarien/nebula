using System;
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
                           Password = new Password("letmein")
                       };
        }

        protected override void ModifyEntity(Account entity)
        {
            entity.UserName = "test";
            entity.Password = new Password("secret");
            entity.LastLogOnDate = DateTime.Now.Date;
        }

        protected override void AssertAreEqual(Account expectedEntity, Account actualEntity)
        {
            Assert.AreEqual(expectedEntity.UserName, actualEntity.UserName);
            Assert.AreEqual(expectedEntity.Password, actualEntity.Password);
            Assert.AreEqual(expectedEntity.LastLogOnDate, actualEntity.LastLogOnDate);
        }

        protected override void AssertValidId(Account entity)
        {
            Assert.Greater(entity.Id, 0);
        }
    }
}