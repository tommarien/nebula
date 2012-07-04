namespace Nebula.Infrastructure.Querying
{
    public interface IQueryHandler
    {
    }

    public interface IQueryHandler<in TParameters, out TResult> : IQueryHandler
    {
        TResult Execute(TParameters search);
    }
}