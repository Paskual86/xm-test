using XMCrypto.Core.Services.Providers.Abstractions;
using XMCrypto.Core.Services.Providers.Bitstamp.Dto;
using XMCrypto.Domain.Interfaces.Services.Providers;

namespace XMCrypto.Core.Services.Providers.Bitstamp
{
    public class BitstampProvider : BaseProvider<TickerResponseDto>, IBTCProviderService
    {
        public const string CLIENT_API_NAME = "BitstampApi";
        public string Name => "Bitstamp";

        public BitstampProvider(IHttpClientFactory httpCF) : base(httpCF, "/ticker/btcusd/")
        {
            ClientApiName = CLIENT_API_NAME;
        }

        public async Task<IBTCTickerDto> GetTickerAsync()
        {
            return await base.GetCommonTickerAsync();
        }
    }
}
