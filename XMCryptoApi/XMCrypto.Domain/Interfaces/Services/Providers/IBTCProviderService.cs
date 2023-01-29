using XMCrypto.Domain.Enums;

namespace XMCrypto.Domain.Interfaces.Services.Providers
{
    public interface IBTCProviderService
        
    {
        string Name { get; }
        string Path { get; }
        string UrlProvider { get; }
        /// <summary>
        /// Checkout if the service is available
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BTCServiceException"></exception>
        /// <exception cref="Exception"></exception>
        Task<ExternalServiceStatus> GetStatusOfServiceAsync();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IBTCTickerDto> GetTickerAsync();
    }
}
