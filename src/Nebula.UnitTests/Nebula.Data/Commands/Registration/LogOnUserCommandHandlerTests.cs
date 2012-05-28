using NUnit.Framework;
using Nebula.Contracts.Commands.Registration;
using Nebula.Data.Commands.Registration;
using Nebula.Domain.Registration;
using Nebula.Domain.Registration.Queries;
using Rhino.Mocks;

namespace Nebula.UnitTests.Nebula.Data.Commands.Registration
{
    [TestFixture]
    public class LogOnUserCommandHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            query = MockRepository.GenerateMock<IGetUserAccountByUserNameQuery>();
            commandHandler = new LogOnUserCommandHandler(query);
            command = new LogOnUserCommand
                          {
                              UserName = "userx",
                              Password = "secret"
                          };
        }

        private LogOnUserCommandHandler commandHandler;
        private IGetUserAccountByUserNameQuery query;
        private LogOnUserCommand command;

        [Test]
        public void Should_invoke_the_query_as_expected()
        {
            query.Expect(q => q.Execute(command.UserName)).Return(new UserAccount());

            commandHandler.Handle(command);

            query.VerifyAllExpectations();
        }

        [Test]
        public void Should_return_false_if_the_password_does_not_match()
        {
            query.Stub(q => q.Execute(command.UserName)).Return(new UserAccount {Password = "anothersecret"});

            bool result = commandHandler.Handle(command);

            Assert.IsFalse(result);
        }

        [Test]
        public void Should_return_false_if_user_does_not_exist()
        {
            query.Stub(q => q.Execute(command.UserName)).Return(null);

            bool result = commandHandler.Handle(command);

            Assert.IsFalse(result);
        }

        [Test]
        public void Should_return_true_if_the_password_matches()
        {
            query.Stub(q => q.Execute(command.UserName)).Return(new UserAccount {Password = command.Password});

            bool result = commandHandler.Handle(command);

            Assert.IsTrue(result);
        }
    }
}