namespace Nebula.Infrastructure.Querying
{
    public interface IQuery
    {
    }

    public interface IQuery<in TSearch, out TResult> : IQuery
    {
        TResult Execute(TSearch input);
    }
}