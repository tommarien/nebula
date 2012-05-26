using NHibernate;
using NUnit.Framework;
using Nebula.Services.Modules;
using Rhino.Mocks;

namespace Nebula.UnitTests.Nebula.Services.Modules
{
    [TestFixture]
    public class SessionManagerTests
    {
        [SetUp]
        public void Setup()
        {
            sessionFactoryMock = MockRepository.GenerateStrictMock<ISessionFactory>();
            sessionManager = MockRepository.GeneratePartialMock<SessionManager>(sessionFactoryMock);
        }

        private ISessionFactory sessionFactoryMock;
        private SessionManager sessionManager;

        [Test]
        public void Ctor_Should_set_sessionFactory()
        {
            Assert.AreEqual(sessionFactoryMock, sessionManager.SessionFactory);
        }

        [Test]
        public void Dispose_Should_dispose_sessionfactory()
        {
            sessionFactoryMock.Expect(sf => sf.Dispose());

            sessionManager.Dispose();
        }

        [Test]
        public void GetSession_Should_invoke_OpenSession_if_none_is_active_and_return_it()
        {
            var session = MockRepository.GenerateStub<ISession>();
            sessionManager.Expect(sm => sm.OpenSession()).Return(session);

            var verify = sessionManager.GetSession();

            Assert.AreSame(session, verify);
            sessionManager.VerifyAllExpectations();
        }

        [Test]
        public void GetSession_Should_return_the_current_session_if_one_is_active()
        {
            var session = MockRepository.GenerateStub<ISession>();
            sessionManager.Stub(sm => sm.HasActive).Return(true);
            sessionManager.Expect(sm => sm.ActiveSession).Return(session);

            var verify = sessionManager.GetSession();
            Assert.AreSame(session, verify);
            sessionManager.VerifyAllExpectations();
        }

        [Test]
        public void GetSession_Should_set_active_session_if_none_is_active()
        {
            var session = MockRepository.GenerateStub<ISession>();
            sessionManager.Stub(sm => sm.OpenSession()).Return(session);

            sessionManager.GetSession();

            Assert.AreSame(session, sessionManager.ActiveSession);
        }

        [Test]
        public void HasActive_Should_return_true_if_a_session_is_active()
        {
            var session = MockRepository.GenerateStub<ISession>();
            sessionManager.Stub(sm => sm.ActiveSession).Return(session);

            Assert.IsTrue(sessionManager.HasActive);
        }

        [Test]
        public void OpenSession_Should_open_a_new_session_and_return_it()
        {
            var session = MockRepository.GenerateStub<ISession>();
            sessionFactoryMock.Expect(f => f.OpenSession()).Return(session);

            var verify = sessionManager.OpenSession();

            Assert.AreSame(session, verify);
            sessionFactoryMock.VerifyAllExpectations();
        }
    }
}