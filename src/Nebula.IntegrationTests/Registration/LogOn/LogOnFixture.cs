using Nebula.Contracts.Registration.Commands;
using Nebula.Data.Registration.Commands;

namespace Nebula.IntegrationTests.Registration.LogOn
{
    public abstract class LogOnFixture : AutoRollbackFixture
    {
        public LogOnScenario Scenario { get; set; }
        public LogOnUserCommand Command { get; set; }
        public LogOnUserCommandHandler Handler { get; set; }

        protected override void AfterSetUp()
        {
            base.AfterSetUp();

            Scenario = new LogOnScenario();
            Scenario.Populate(Session);

            Command = new LogOnUserCommand
                {
                    UserName = Scenario.JohnDoe.UserName,
                    Password = "superDuper12"
                };

            Handler = new LogOnUserCommandHandler(Session);
        }

        protected void Act()
        {
            Handler.Handle(Command);
        }
    }
}