using System;
using Castle.Core.Logging;
using Castle.DynamicProxy;

namespace Nebula.Infrastructure.Transactions
{
    public class AutomaticTransactionInterceptor : IInterceptor
    {
        private readonly ITransactionManager transactionManager;
        private ILogger logger;

        public AutomaticTransactionInterceptor(ITransactionManager transactionManager)
        {
            this.transactionManager = transactionManager;
        }

        public ILogger Logger
        {
            get { return logger = logger ?? NullLogger.Instance; }
            set { logger = value; }
        }

        public bool HasStartedTransaction { get; set; }

        public void Intercept(IInvocation invocation)
        {
            if (!transactionManager.IsTransactionActive)
            {
                transactionManager.Begin();
                HasStartedTransaction = true;
                Logger.InfoFormat("{0}.{1} started a transaction", invocation.Method.GetType().Name, invocation.Method.Name);
            }

            try
            {
                invocation.Proceed();
                
                if (!HasStartedTransaction) return;

                transactionManager.Commit();
                Logger.InfoFormat("{0}.{1} committed the transaction", invocation.Method.GetType().Name, invocation.Method.Name);
            }
            catch (Exception e)
            {
                if (!HasStartedTransaction) throw;

                transactionManager.Rollback();
                Logger.InfoFormat("{0}.{1} Rollback current transaction because of exception", invocation.Method.GetType().Name, invocation.Method.Name);
                if (!(e is AbortTransactionException)) throw;
            }
        }
    }
}