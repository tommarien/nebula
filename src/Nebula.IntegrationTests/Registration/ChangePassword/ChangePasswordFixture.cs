using Nebula.Contracts.Registration.Commands;
using Nebula.Data.Registration.Commands;
using Nebula.IntegrationTests.Registration.LogOn;

namespace Nebula.IntegrationTests.Registration.ChangePassword
{
    public abstract class ChangePasswordFixture : AutoRollbackFixture
    {
        public LogOnScenario Scenario { get; set; }
        public ChangePasswordCommand Command { get; set; }
        public ChangePasswordCommandHandler Handler { get; set; }

        protected override void AfterSetUp()
        {
            base.AfterSetUp();

            Scenario = new LogOnScenario();
            Scenario.Populate(Session);

            Command = new ChangePasswordCommand
                {
                    UserName = Scenario.JohnDoe.UserName,
                    OldPassword = "superDuper12",
                    NewPassword = "changed"
                };

            Handler = new ChangePasswordCommandHandler(Session);
        }

        protected bool Act()
        {
            return Handler.Handle(Command);
        }
    }
}