using System;
using Castle.DynamicProxy;
using NUnit.Framework;
using Nebula.Infrastructure;
using Nebula.Infrastructure.Transactions;
using Rhino.Mocks;

namespace Nebula.UnitTests.Nebula.Infrastructure
{
    [TestFixture]
    public class AutomaticTransactionInterceptorTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            transactionManager = MockRepository.GenerateMock<ITransactionManager>();
            interceptor = new AutomaticTransactionInterceptor(transactionManager);
            methodIntercepted = MockRepository.GenerateStub<IInvocation>();
        }

        #endregion

        private ITransactionManager transactionManager;
        private AutomaticTransactionInterceptor interceptor;
        private IInvocation methodIntercepted;

        [Test]
        public void Intercept_Should_begin_a_transaction_if_none_is_active()
        {
            transactionManager.Stub(m => m.IsTransactionActive).Return(false);
            transactionManager.Expect(m => m.Begin());

            interceptor.Intercept(methodIntercepted);

            transactionManager.VerifyAllExpectations();
        }

        [Test]
        public void Intercept_Should_commit_the_transaction_if_it_started_it()
        {
            transactionManager.Stub(m => m.IsTransactionActive).Return(false);
            transactionManager.Expect(m => m.Begin());
            transactionManager.Expect(m => m.Commit());

            interceptor.Intercept(methodIntercepted);

            transactionManager.VerifyAllExpectations();
        }

        [Test]
        public void Intercept_Should_not_begin_a_transaction_if_one_is_active()
        {
            transactionManager.Stub(m => m.IsTransactionActive).Return(true);

            interceptor.Intercept(methodIntercepted);

            transactionManager.AssertWasNotCalled(s => s.Begin());
        }

        [Test]
        public void Intercept_Should_not_commit_the_transaction_if_it_did_not_start_it()
        {
            transactionManager.Stub(m => m.IsTransactionActive).Return(true);

            interceptor.Intercept(methodIntercepted);

            transactionManager.AssertWasNotCalled(s => s.Commit());
        }

        [Test]
        public void Intercept_Should_not_rollback_a_transaction_on_error_when_it_did_not_start_it()
        {
            transactionManager.Stub(m => m.IsTransactionActive).Return(true);

            methodIntercepted = MockRepository.GenerateStub<IInvocation>();
            methodIntercepted.Stub(i => i.Proceed()).Throw(new Exception());

            Assert.Throws<Exception>(() => interceptor.Intercept(methodIntercepted));
            transactionManager.AssertWasNotCalled(t => t.Rollback());
        }

        [Test]
        public void Intercept_Should_proceed_in_the_right_order()
        {
            var mocks = new MockRepository();
            transactionManager = mocks.StrictMock<ITransactionManager>();
            interceptor = new AutomaticTransactionInterceptor(transactionManager);
            methodIntercepted = mocks.StrictMock<IInvocation>();

            using (mocks.Ordered())
            {
                transactionManager.Expect(t => t.IsTransactionActive).Return(false);
                transactionManager.Expect(t => t.Begin());
                methodIntercepted.Expect(i => i.Proceed());
                transactionManager.Expect(t => t.Commit());
            }

            mocks.ReplayAll();

            interceptor.Intercept(methodIntercepted);

            mocks.VerifyAll();
        }

        [Test]
        public void Intercept_Should_rollback_a_started_transaction_but_do_not_throw_when_an_abortTransactionException_is_thrown()
        {
            transactionManager.Stub(t => t.IsTransactionActive).Return(false);
            methodIntercepted.Stub(i => i.Proceed()).Throw(new AbortTransactionException());
            transactionManager.Stub(t => t.Begin());
            transactionManager.Expect(t => t.Rollback());

            Assert.DoesNotThrow(() => interceptor.Intercept(methodIntercepted));

            transactionManager.VerifyAllExpectations();
        }

        [Test]
        public void Intercept_Should_rollback_a_started_transaction_when_something_goes_wrong()
        {
            transactionManager.Stub(t => t.IsTransactionActive).Return(false);
            methodIntercepted.Stub(i => i.Proceed()).Throw(new Exception());
            transactionManager.Stub(t => t.Begin());
            transactionManager.Expect(t => t.Rollback());

            Assert.Throws<Exception>(() => interceptor.Intercept(methodIntercepted));
            transactionManager.VerifyAllExpectations();
        }
    }
}