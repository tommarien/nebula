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

        private IInvocation Owner { get; set; }

        public void Intercept(IInvocation invocation)
        {
            BeginTransactionIfNeeded(invocation);

            try
            {
                invocation.Proceed();
                CommitTransactionIfNeeded(invocation);
            }
            catch (Exception)
            {
                RollBackTransactionIfNeeded(invocation);
                throw;
            }
        }

        private bool IsTransactionOwner(IInvocation invocation)
        {
            return (Owner == invocation);
        }

        private void BeginTransactionIfNeeded(IInvocation invocation)
        {
            if (session.Value.Transaction.IsActive) return;
            session.Value.BeginTransaction();
            Owner = invocation;
        }

        private void CommitTransactionIfNeeded(IInvocation invocation)
        {
            if (!IsTransactionOwner(invocation)) return;
            session.Value.Transaction.Commit();
            Owner = null;
        }

        private void RollBackTransactionIfNeeded(IInvocation invocation)
        {
            if (!IsTransactionOwner(invocation)) return;
            session.Value.Transaction.Rollback();
            Owner = null;
        }
    }
}