using System;
using Castle.DynamicProxy;
using NHibernate;
using IInterceptor = Castle.DynamicProxy.IInterceptor;

namespace Nebula.Bootstrapper
{
    public class AutoTransactionInterceptor : IInterceptor
    {
        private readonly ISession session;

        public AutoTransactionInterceptor(ISession session)
        {
            this.session = session;
        }

        public void Intercept(IInvocation invocation)
        {
            ITransaction transaction = null;

            if (!session.Transaction.IsActive)
                transaction = session.BeginTransaction();

            try
            {
                invocation.Proceed();
                if (transaction != null) transaction.Commit();
            }
            catch (Exception)
            {
                if (transaction != null) transaction.Rollback();
                throw;
            }
        }
    }
}