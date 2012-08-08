namespace Nebula.Infrastructure.Querying
{
    public interface IQueryHandlerFactory
    {
        IQueryHandler<TQuery, TResult> CreateHandler<TQuery, TResult>();
        void Release(IQueryHandler query);
    }
}