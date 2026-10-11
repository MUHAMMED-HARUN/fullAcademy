using System.Linq.Expressions;
namespace System.Application.Abstractions
{
    public interface IGeneric <TEntity>
    {
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true);
    }
}
