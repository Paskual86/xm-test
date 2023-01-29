using XMCrypto.Domain.Enums;

namespace XMCrypto.Domain.Interfaces.Services.Providers
{
    public interface IBTCProviderService
        
    {
        string Name { get; }
        string Path { get; }
        string UrlProvider { get; }
        Task<decimal> GetPriceAsync();
        Task<ExternalServiceStatus> GetStatusOfServiceAsync();
        Task<IBTCTickerDto> GetTickerAsync();
    }
}
