using XMCrypto.Domain.Entities;
using XMCrypto.Domain.Interfaces.Repository;
using XMCrypto.Persistance.Repositories.Abstractions;

namespace XMCrypto.Persistance.Repositories
{
    public class BTCRepository : BaseRepository<BitCoinPrice, long>, IBTCRepository
    {
        public BTCRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
