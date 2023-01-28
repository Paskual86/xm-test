using XMCrypto.Domain.Enums;
using XMCrypto.Domain.Interfaces.Services.Providers;

namespace XMCrypto.Core.Services.Providers.Bitfinex
{
    public class BitfinexProvider : IBTCProviderService
    {
        public string Name => "Bitfinex";
        public string UrlProvider { get; private set; }

        public BitfinexProvider()
        {
            UrlProvider = "https://api.bitfinex.com/v1/pubticker/BTCUSD";
        }

        public async Task<decimal> GetPriceAsync()
        {
            var response = await GetTickerAsync();
            return response.LastPrice;
        }

        public Task<ExternalServiceStatus> GetStatusOfServiceAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IBTCTickerDto> GetTickerAsync()
        {
            throw new NotImplementedException();
        }
    }
}
