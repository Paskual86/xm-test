using XMCrypto.Core.Services.Providers.Abstractions;
using XMCrypto.Core.Services.Providers.Bitfinex.Dto;
using XMCrypto.Domain.Interfaces.Services.Providers;

namespace XMCrypto.Core.Services.Providers.Bitfinex
{
    public class BitfinexProvider : BaseProvider<TickerResponseDto>, IBTCProviderService
    {
        public const string CLIENT_API_NAME = "BitfinexApi";
        private const string PATH = "v1/pubticker/BTCUSD";
        private const string NAME = "Bitfinex";
        
        public BitfinexProvider(IHttpClientFactory httpCF): base(httpCF)
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
