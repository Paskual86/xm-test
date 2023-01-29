using XMCrypto.Domain.Enums;

namespace XMCrypto.Domain.Interfaces.Services.Providers
{
    public interface IBTCProviderService<TDto>
        where TDto : class, IBTCTickerDto
    {
        string Name { get; }
        string UrlProvider { get; }
        Task<decimal> GetPriceAsync();
        Task<ExternalServiceStatus> GetStatusOfServiceAsync();
        Task<TDto> GetTickerAsync();
    }
}
