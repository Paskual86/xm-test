using XMCrypto.Core.Services.Providers.Abstractions;
using XMCrypto.Core.Services.Providers.Bitstamp.Dto;
using XMCrypto.Domain.Interfaces.Services.Providers;

namespace XMCrypto.Core.Services.Providers.Bitstamp
{
    public class BitstampProvider : BaseProvider<TickerResponseDto>, IBTCProviderService
    {
        public const string CLIENT_API_NAME = "BitstampApi";
        private const string PATH = "api/v2/ticker/btcusd/";
        private const string NAME = "Bitstamp";

        public BitstampProvider(IHttpClientFactory httpCF) : base(httpCF)
        {
            ClientApiName = CLIENT_API_NAME;
            Path = PATH;
            Name = NAME;
        }

        public async Task<IBTCTickerDto> GetTickerAsync()
        {
            return await base.GetCommonTickerAsync();
        }
    }
}
