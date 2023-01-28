namespace XMCrypto.Domain.Interfaces.Services
{
    public interface IBTCService
    {
        Task GetSourceAvailablesAsync();
        Task FetchPriceAsync(string source);
        Task GetAllHistoryPrice();
        Task GetHistoryPrice(string source);
    }
}
