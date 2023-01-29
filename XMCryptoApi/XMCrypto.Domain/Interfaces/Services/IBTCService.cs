using XMCrypto.Domain.Entities;

namespace XMCrypto.Domain.Interfaces.Services
{
    public interface IBTCService
    {
        Task<IList<CryptoProvider>> GetSourceAvailablesAsync();
        Task<BitCoinPrice> FetchPriceAsync(string source);
        Task<IList<BitCoinPrice>> GetAllHistoryPrice();
        Task<IList<BitCoinPrice>> GetHistoryPrice(string source);
    }
}
