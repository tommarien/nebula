namespace Nebula.Infrastructure.Querying
{
    public interface IQueryHandler
    {
    }

    public interface IQueryHandler<in TQuery, out TResult> : IQueryHandler
    {
        TResult Execute(TQuery query);
    }
}