using System;
using Castle.Core.Logging;
using Castle.DynamicProxy;
using NHibernate;
using Nebula.Infrastructure;
using IInterceptor = Castle.DynamicProxy.IInterceptor;
using ILoggerFactory = Castle.Core.Logging.ILoggerFactory;

namespace Nebula.Bootstrapper.Interceptors
{
    public class AutoTransactionInterceptor : IInterceptor
    {
        private readonly ISession session;

        public AutoTransactionInterceptor(ISession session, ILoggerFactory loggerFactory)
        {
            this.session = session;
            Logger = loggerFactory.Create("Nebula.AutoTx");
        }

        public ILogger Logger { get; private set; }

        public void Intercept(IInvocation invocation)
        {
            ITransaction transaction = null;

            if (!session.Transaction.IsActive)
            {
                transaction = session.BeginTransaction();
                Logger.Info("BEGIN");
            }

            try
            {
                invocation.Proceed();
                if (transaction != null)
                {
                    transaction.Commit();
                    Logger.Info("COMMIT");
                }
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                    Logger.Info("ROLLBACK");
                    if (!(e is AbortTransactionException)) throw;
                }
                else throw;
            }
        }
    }
}