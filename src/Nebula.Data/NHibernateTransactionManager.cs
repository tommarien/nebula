using System.Data;
using NHibernate;
using Nebula.Infrastructure.Transactions;

namespace Nebula.Data
{
    public class NHibernateTransactionManager : ITransactionManager
    {
        private readonly ISession session;

        public NHibernateTransactionManager(ISession session)
        {
            this.session = session;
        }

        public bool IsTransactionActive
        {
            get { return session.Transaction.IsActive; }
        }

        public void Begin()
        {
            session.Transaction.Begin();
        }

        public void Begin(IsolationLevel isolationLevel)
        {
            session.Transaction.Begin(isolationLevel);
        }

        public void Commit()
        {
            session.Transaction.Commit();
        }

        public void Rollback()
        {
            session.Transaction.Rollback();
        }
    }
}