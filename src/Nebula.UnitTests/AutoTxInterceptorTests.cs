using System;
using Castle.DynamicProxy;
using NHibernate;
using NSubstitute;
using NUnit.Framework;
using Nebula.Bootstrapper.Interceptors;

namespace Nebula.UnitTests
{
    [TestFixture]
    public class AutoTxInterceptorTests
    {
        [SetUp]
        public void Setup()
        {
            session = Substitute.For<ISession>();
            session.Transaction.Returns(Substitute.For<ITransaction>());
            interceptor = new AutoTxInterceptor(new Lazy<ISession>(() => session));
            invocation = Substitute.For<IInvocation>();
        }

        private AutoTxInterceptor interceptor;
        private ISession session;
        private IInvocation invocation;

        [Test]
        public void Intercept_Should_begin_a_transaction_if_none_is_active()
        {
            session.Transaction.IsActive.Returns(false);
            session.BeginTransaction().Returns(session.Transaction);

            interceptor.Intercept(invocation);

            session.Received().BeginTransaction();
        }

        [Test]
        public void Intercept_Should_commit_the_transaction_if_it_started_it()
        {
            session.Transaction.IsActive.Returns(false);
            session.BeginTransaction().Returns(session.Transaction);

            interceptor.Intercept(invocation);

            session.Transaction.Received().Commit();
        }

        [Test]
        public void Intercept_Should_not_begin_a_transaction_if_one_is_active()
        {
            session.Transaction.IsActive.Returns(true);

            interceptor.Intercept(invocation);

            session.DidNotReceive().BeginTransaction();
        }

        [Test]
        public void Intercept_Should_not_commit_the_transaction_if_it_did_not_start_it()
        {
            session.Transaction.IsActive.Returns(true);

            interceptor.Intercept(invocation);

            session.Transaction.DidNotReceive().Commit();
        }

        [Test]
        public void Intercept_Should_not_rollback_a_transaction_on_error_when_it_did_not_start_it()
        {
            session.Transaction.IsActive.Returns(true);
            invocation.When(inv => inv.Proceed()).Do(info => { throw new Exception(); });

            Assert.Throws<Exception>(() => interceptor.Intercept(invocation));

            session.Transaction.DidNotReceive().Rollback();
        }

        [Test]
        public void Intercept_Should_proceed_the_invocation()
        {
            session.Transaction.IsActive.Returns(true);

            interceptor.Intercept(invocation);

            invocation.Received().Proceed();
        }

        [Test]
        public void Intercept_Should_rollback_a_started_transaction_when_something_goes_wrong()
        {
            session.Transaction.IsActive.Returns(false);
            session.BeginTransaction().Returns(session.Transaction);

            invocation.When(inv => inv.Proceed()).Do(info => { throw new Exception(); });

            Assert.Throws<Exception>(() => interceptor.Intercept(invocation));

            session.Transaction.Received().Rollback();
        }
    }
}