using NHibernate;
using Nebula.Contracts.Registration;
using Nebula.Domain.Registration;
using Nebula.UnitTests.Builders;

namespace Nebula.IntegrationTests.Registration.LogOn
{
    public class LogOnScenario
    {
        public Account JohnDoe { get; set; }
        public Account LaraCroft { get; set; }

        public virtual void Populate(ISession session)
        {
            JohnDoe = new AccountBuilder()
                .Named("JohnDoe")
                .WithPassword("superDuper12")
                .WithRole(Role.Administrator)
                .Build();

            LaraCroft = new AccountBuilder()
                .Named("LaraCroft")
                .WithPassword("superDuper12")
                .Build();

            session.Save(JohnDoe);
            session.Save(LaraCroft);
        }
    }
}