using XMCrypto.Core.Services.Providers.Abstractions;
using XMCrypto.Core.Services.Providers.Bitfinex.Dto;
using XMCrypto.Domain.Interfaces.Services.Providers;

namespace XMCrypto.Core.Services.Providers.Bitfinex
{
    public class BitfinexProvider : BaseProvider<TickerResponseDto>, IBTCProviderService
    {
        public const string CLIENT_API_NAME = "BitfinexApi";
        public const string PATH = "v1/pubticker/BTCUSD";
        public string Name => "Bitfinex";
        public BitfinexProvider(IHttpClientFactory httpCF): base(httpCF)
        {
            ClientApiName = CLIENT_API_NAME;
            Path = PATH;
        }

        public async Task<IBTCTickerDto> GetTickerAsync()
        {
            return await base.GetCommonTickerAsync();
        }
    }
}
