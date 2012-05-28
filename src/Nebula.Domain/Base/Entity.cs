namespace Nebula.Domain.Base
{
    public interface IEntity
    {
    }

    public interface IEntity<out TKey>
    {
        TKey Id { get; }
    }

    public abstract class Entity<TKey> : IEntity<TKey>
    {
        public virtual TKey Id { get; protected set; }
    }
}