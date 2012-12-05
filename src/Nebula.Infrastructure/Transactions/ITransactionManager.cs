using System.Data;

namespace Nebula.Infrastructure.Transactions
{
    public interface ITransactionManager
    {
        bool IsTransactionActive { get; }

        void Begin();
        void Begin(IsolationLevel isolationLevel);

        void Commit();
        void Rollback();
    }
}