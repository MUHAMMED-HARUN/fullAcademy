using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions;
using System.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace System.Infrastructure.Repositories.Address
{
    public class GenericService<TEntity> : IGeneric<TEntity> where TEntity : class
    {
        private readonly AppDbContext _context;

        public GenericService(AppDbContext context)
        {
            _context = context;

        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true)
        {
            var query = _context.Set<TEntity>().Where(predicate);

            if (asNoTracking)
                query = query.AsNoTracking();

            return query;
        }
    }
}
