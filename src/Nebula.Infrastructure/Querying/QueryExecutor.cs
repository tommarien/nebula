namespace Nebula.Infrastructure.Querying
{
    public class QueryExecutor : IQueryExecutor
    {
        private readonly IQueryFactory queryFactory;

        public QueryExecutor(IQueryFactory queryFactory)
        {
            this.queryFactory = queryFactory;
        }

        public virtual TResult Execute<TSearch, TResult>(TSearch search)
        {
            var query = queryFactory.CreateQuery<TSearch, TResult>();

            try
            {
                return query.Execute(search);
            }
            finally
            {
                queryFactory.Release(query);
            }
        }
    }
}