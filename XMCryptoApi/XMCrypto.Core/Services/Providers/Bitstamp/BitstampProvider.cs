using XMCrypto.Core.Services.Providers.Abstractions;
using XMCrypto.Domain.Interfaces.Services.Providers;

namespace XMCrypto.Core.Services.Providers.Bitstamp
{
    public class BitstampProvider : BaseProvider, IBTCProviderService
    {
        public const string CLIENT_API_NAME = "BitstampApi";
        public string Name => "Bitstamp";

        public BitstampProvider(IHttpClientFactory httpCF) : base(httpCF, "v2/ticker/btcusd/")
        {
            ClientApiName = CLIENT_API_NAME;
        }

        public async Task<decimal> GetPriceAsync()
        {
            var response = await GetTickerAsync();
            return response.LastPrice;
        }

        public Task<IBTCTickerDto> GetTickerAsync()
        {
            throw new NotImplementedException();
        }
    }
}
