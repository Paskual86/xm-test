using XMCrypto.Core.Services.Providers.Abstractions;
using XMCrypto.Core.Services.Providers.Bitfinex.Dto;
using XMCrypto.Domain.Interfaces.Services.Providers;

namespace XMCrypto.Core.Services.Providers.Bitfinex
{
    public class BitfinexProvider : BaseProvider<TickerResponseDto>, IBTCProviderService<TickerResponseDto>
    {
        public const string CLIENT_API_NAME = "BitfinexApi";
        public string Name => "Bitfinex";
        
        public BitfinexProvider(IHttpClientFactory httpCF): base(httpCF, "v1/pubticker/BTCUSD")
        {
            ClientApiName = CLIENT_API_NAME;
        }
    }
}
