namespace Nebula.Infrastructure.Querying
{
    public interface IQuery
    {
    }

    public interface IQuery<in TParameters, out TResult> : IQuery
    {
        TResult Execute(TParameters parameters);
    }
}