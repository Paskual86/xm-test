using Microsoft.EntityFrameworkCore;
using XMCrypto.Domain.Abstractions;
using XMCrypto.Domain.Exceptions;

namespace XMCrypto.Persistance.Abstractions
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext context;

        public UnitOfWork(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public async Task<bool> Commit()
        {
            try
            {
                return (await context.SaveChangesAsync()) >= 0;
            }
            catch (Exception ex)
            {
                throw new PersistanceException(ex.Message, PersistanceException.COMMIT_ERROR);
            }
            
        }
    }
}
