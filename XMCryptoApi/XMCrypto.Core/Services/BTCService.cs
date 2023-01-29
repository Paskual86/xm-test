using XMCrypto.Domain.Entities;
using XMCrypto.Domain.Interfaces.Services;
using XMCrypto.Domain.Interfaces.Services.Providers;

namespace XMCrypto.Core.Services
{
    public class BTCService : IBTCService
    {
        private readonly IEnumerable<IBTCProviderService<IBTCTickerDto>> btcProvider;

        public BTCService(IEnumerable<IBTCProviderService<IBTCTickerDto>> btcProv)
        {
            btcProvider = btcProv;
        }
        public Task FetchPriceAsync(string source)
        {
            throw new NotImplementedException();
        }

        public Task<IList<BitCoinPrice>> GetAllHistoryPrice()
        {
            throw new NotImplementedException();
        }

        public Task<IList<BitCoinPrice>> GetHistoryPrice(string source)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<CryptoProvider>> GetSourceAvailablesAsync()
        {

            // First  - Load all implementations of IBTCProviderService
            // Second - Loop the values and check out if the service is available
            // Third  - Add The Valid Url to the result
            var result = new List<CryptoProvider>();

            if (btcProvider == null) {
                throw new NotImplementedException("No Implementations of Providers");
            }

            foreach (var provider in btcProvider) 
            {
                try
                {
                    if (await provider.GetStatusOfServiceAsync() == Domain.Enums.ExternalServiceStatus.Available) 
                    {
                        result.Add(new CryptoProvider()
                        {
                            Source = provider.Name,
                            Url = provider.UrlProvider
                        });
                    }
                }
                catch 
                { 
                    // For Now we dont throw any exception
                }
            }
            return await Task.FromResult(result);
        }
    }
}
