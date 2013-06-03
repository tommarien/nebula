using System;
using Castle.DynamicProxy;
using NHibernate;
using IInterceptor = Castle.DynamicProxy.IInterceptor;

namespace Nebula.Bootstrapper.Interceptors
{
    public class AutoTxInterceptor : IInterceptor
    {
        private readonly Lazy<ISession> session;

        public AutoTxInterceptor(Lazy<ISession> session)
        {
            this.session = session;
        }

        private bool OwnsTransaction { get; set; }

        public void Intercept(IInvocation invocation)
        {
            BeginTransactionIfNeeded();

            try
            {
                invocation.Proceed();
                CommitTransactionIfNeeded();
            }
            catch (Exception)
            {
                RollBackTransactionIfNeeded();
                throw;
            }
        }

        private void BeginTransactionIfNeeded()
        {
            if (session.Value.Transaction.IsActive) return;
            session.Value.BeginTransaction();
            OwnsTransaction = true;
        }

        private void CommitTransactionIfNeeded()
        {
            if (!OwnsTransaction) return;
            session.Value.Transaction.Commit();
        }

        private void RollBackTransactionIfNeeded()
        {
            if (!OwnsTransaction) return;
            session.Value.Transaction.Rollback();
        }
    }
}