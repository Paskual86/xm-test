using XMCrypto.Domain.Enums;
using XMCrypto.Domain.Interfaces.Services.Providers;

namespace XMCrypto.Core.Services.Providers.Bitstamp
{
    public class BitstampProvider : IBTCProviderService
    {
        public string Name => "Bitstamp";
        public string UrlProvider { get; private set; }

        public BitstampProvider()
        {
            UrlProvider = "​https://www.bitstamp.net/api/v2/ticker/btcusd/";
        }

        public decimal GetPrice()
        {
            throw new NotImplementedException();
        }

        public ExternalServiceStatus GetStatusOfService()
        {
            throw new NotImplementedException();
        }
    }
}
