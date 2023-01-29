using XMCrypto.Domain.Enums;
using XMCrypto.Domain.Interfaces.Services.Providers;

namespace XMCrypto.Core.Services.Providers.Bitstamp
{
    public class BitstampProvider : IBTCProviderService
    {
        public const string CLIENT_API_NAME = "BitstampApi";
        public string Name => "Bitstamp";
        public string UrlProvider { get; private set; }

        public BitstampProvider()
        {
            UrlProvider = "v2/ticker/btcusd/";
        }

        public async Task<decimal> GetPriceAsync()
        {
            var response = await GetTickerAsync();
            return response.LastPrice;
        }

        public async Task<ExternalServiceStatus> GetStatusOfServiceAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IBTCTickerDto> GetTickerAsync()
        {
            throw new NotImplementedException();
        }
    }
}
