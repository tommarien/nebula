using System;
using Castle.DynamicProxy;

namespace Nebula.Infrastructure.Transactions
{
    public class AutomaticTransactionInterceptor : IInterceptor
    {
        private readonly ITransactionManager transactionManager;

        public AutomaticTransactionInterceptor(ITransactionManager transactionManager)
        {
            this.transactionManager = transactionManager;
        }

        public bool HasStartedTransaction { get; set; }

        public void Intercept(IInvocation invocation)
        {
            if (!transactionManager.IsTransactionActive)
            {
                transactionManager.Begin();
                HasStartedTransaction = true;
            }

            try
            {
                invocation.Proceed();

                if (HasStartedTransaction) 
                    transactionManager.Commit();
            }
            catch (Exception e)
            {
                if (!HasStartedTransaction) throw;

                transactionManager.Rollback();
                if (!(e is AbortTransactionException)) throw;
            }
        }
    }
}