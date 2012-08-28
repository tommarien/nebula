using System;
using Castle.Core.Logging;
using Castle.DynamicProxy;
using NHibernate;
using NUnit.Framework;
using Nebula.Bootstrapper.Interceptors;
using Rhino.Mocks;
using ILoggerFactory = Castle.Core.Logging.ILoggerFactory;

namespace Nebula.UnitTests.Nebula.Bootstrapper.Interceptors
{
    [TestFixture]
    public class AutoTransactionInterceptorTests
    {
        [SetUp]
        public void Setup()
        {
            loggerFactory = MockRepository.GenerateStub<ILoggerFactory>();
            loggerFactory.Stub(lf => lf.Create(Arg<string>.Is.Anything)).Return(MockRepository.GenerateStub<ILogger>());
            session = MockRepository.GenerateMock<ISession>();
            transaction = MockRepository.GenerateMock<ITransaction>();

            session.Stub(s => s.Transaction).Do(new Func<ITransaction>(() => transaction));
            autoTransactionInterceptor = new AutoTransactionInterceptor(session, loggerFactory);
            invocation = MockRepository.GenerateStub<IInvocation>();
        }

        private AutoTransactionInterceptor autoTransactionInterceptor;
        private ISession session;
        private ILoggerFactory loggerFactory;
        private ITransaction transaction;
        private IInvocation invocation;

        [Test]
        public void Intercept_Should_begin_a_transaction_if_none_is_active()
        {
            transaction.Stub(t => t.IsActive).Return(false);
            session.Expect(s => s.BeginTransaction()).Return(transaction);

            autoTransactionInterceptor.Intercept(invocation);

            session.VerifyAllExpectations();
        }

        [Test]
        public void Intercept_Should_commit_the_transaction_if_it_started_it()
        {
            transaction.Stub(t => t.IsActive).Return(false);
            session.Expect(s => s.BeginTransaction()).Return(transaction);
            transaction.Expect(t => t.Commit());

            autoTransactionInterceptor.Intercept(invocation);

            transaction.VerifyAllExpectations();
        }

        [Test]
        public void Intercept_Should_not_begin_a_transaction_if_one_is_active()
        {
            transaction.Stub(t => t.IsActive).Return(true);

            autoTransactionInterceptor.Intercept(invocation);

            session.AssertWasNotCalled(s => s.BeginTransaction());
        }

        [Test]
        public void Intercept_Should_not_commit_the_transaction_if_it_did_not_start_it()
        {
            transaction = MockRepository.GenerateStrictMock<ITransaction>();
            transaction.Expect(t => t.IsActive).Return(true);

            autoTransactionInterceptor.Intercept(invocation);

            transaction.VerifyAllExpectations();
        }

        [Test]
        public void Intercept_Should_not_rollback_a_transaction_on_error_when_it_did_not_start_it()
        {
            transaction.Stub(t => t.IsActive).Return(true);

            invocation = MockRepository.GenerateStub<IInvocation>();
            invocation.Stub(i => i.Proceed()).Throw(new Exception());

            Assert.Throws<Exception>(() => autoTransactionInterceptor.Intercept(invocation));
            transaction.AssertWasNotCalled(t => t.Rollback());
        }

        [Test]
        public void Intercept_Should_proceed_in_the_right_order()
        {
            var mocks = new MockRepository();
            transaction = mocks.StrictMock<ITransaction>();
            session = mocks.StrictMock<ISession>();
            autoTransactionInterceptor = new AutoTransactionInterceptor(session, loggerFactory);
            invocation = mocks.StrictMock<IInvocation>();

            using (mocks.Ordered())
            {
                session.Expect(s => s.Transaction).Return(transaction);
                transaction.Expect(t => t.IsActive).Return(false);
                session.Expect(t => t.BeginTransaction()).Return(transaction);
                invocation.Expect(i => i.Proceed());
                transaction.Expect(t => t.Commit());
            }

            mocks.ReplayAll();

            autoTransactionInterceptor.Intercept(invocation);

            mocks.VerifyAll();
        }

        [Test]
        public void Intercept_Should_proceed_the_invocation()
        {
            invocation = MockRepository.GenerateStrictMock<IInvocation>();
            invocation.Expect(i => i.Proceed());

            transaction.Stub(t => t.IsActive).Return(true);

            autoTransactionInterceptor.Intercept(invocation);

            invocation.VerifyAllExpectations();
        }

        [Test]
        public void Intercept_Should_rollback_a_started_transaction_when_something_goes_wrong()
        {
            transaction.Stub(t => t.IsActive).Return(false);

            invocation = MockRepository.GenerateStub<IInvocation>();
            invocation.Stub(i => i.Proceed()).Throw(new Exception());

            session.Stub(s => s.BeginTransaction()).Return(transaction);

            Assert.Throws<Exception>(() => autoTransactionInterceptor.Intercept(invocation));
            transaction.VerifyAllExpectations();
        }
    }
}