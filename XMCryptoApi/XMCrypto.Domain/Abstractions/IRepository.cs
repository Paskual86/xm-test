using System.Linq.Expressions;

namespace XMCrypto.Domain.Abstractions
{
    public interface IRepository<T, Tkey>
        where T : class
    {
        Task<T> GetByIdAsync(Tkey id);
        Task<T> GetAsync(Expression<Func<T, bool>> filter, string[]? includeProperties = null);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);
        Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>>? filter = null, string[]? includes = null, int? take = null);
        Task<bool> ExistAsync(Expression<Func<T, bool>>? filter = null);
        Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);
    }
}
