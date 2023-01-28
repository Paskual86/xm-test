using XMCrypto.Domain.Abstractions;

namespace XMCrypto.Persistance.Abstractions
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Commit()
        {
            return (await _context.SaveChangesAsync()) >= 0;
        }
    }
}
