using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using XMCrypto.Domain.Abstractions;
using XMCrypto.Domain.Exceptions;

namespace XMCrypto.Persistance.Repositories.Abstractions
{
    public abstract class BaseRepository<T, Tkey> : IRepository<T, Tkey>
        where T : class
    {
        private readonly ApplicationDbContext context;

        public BaseRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public async Task AddAsync(T entity)
        {
            try
            {
                await context.AddAsync(entity);
            }
            catch(Exception)
            {
                throw new PersistanceException("There was an error trying to add a new data", PersistanceException.ADDING_ERROR);
            }
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>>? filter = null)
        {
            if (filter == null)
            {
                return await context.Set<T>().CountAsync();
            }

            return await context.Set<T>().CountAsync(filter);
        }

        public async Task<bool> ExistAsync(Expression<Func<T, bool>>? filter = null)
        {
            if (filter == null)
            {
                return await context.Set<T>().AnyAsync();
            }

            return await context.Set<T>().AnyAsync(filter);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter, string[]? includeProperties = null)
        {
            if (filter == null)
            {
                throw new PersistanceException("Arguments could not be null", PersistanceException.ARGUMENTS_NULL);
            }

            if (includeProperties == null)
            {
                return await context.Set<T>().FirstOrDefaultAsync(filter);
            }

            IQueryable<T> query = context.Set<T>();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.FirstOrDefaultAsync(filter);
        }

        public async Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>>? filter = null, string[]? includeProperties = null, int? take = null)
        {
            IQueryable<T> query = context.Set<T>();

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (filter == null)
            {
                return await query.AsNoTracking().ToListAsync();
            }

            return await query.AsNoTracking().Where(filter).Take(take ?? 10).ToListAsync();
        }
        public async Task<T> GetByIdAsync(Tkey id)
        {
            return await context.Set<T>().FindAsync(id);
        }

#pragma warning disable 1998
        // disable async warning
        public virtual async Task RemoveAsync(T entity)
        {
            context.Remove(entity);
        }
#pragma warning restore 1998

#pragma warning disable 1998
        // disable async warning
        public virtual async Task UpdateAsync(T entity)
        {
            context.Update(entity);
        }
#pragma warning restore 1998
    }
}
