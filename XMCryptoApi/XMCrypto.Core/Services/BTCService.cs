using XMCrypto.Core.Services.Exceptions;
using XMCrypto.Domain.Abstractions;
using XMCrypto.Domain.Entities;
using XMCrypto.Domain.Interfaces.Repository;
using XMCrypto.Domain.Interfaces.Services;
using XMCrypto.Domain.Interfaces.Services.Providers;

namespace XMCrypto.Core.Services
{
    public class BTCService : IBTCService
    {
        private readonly IEnumerable<IBTCProviderService> btcProvider;
        private readonly IBTCRepository btcRepository;
        private readonly IUnitOfWork unitOfWork;

        public BTCService(IEnumerable<IBTCProviderService> btcProv, IBTCRepository btcRepo, IUnitOfWork unitOfW)
        {
            btcProvider = btcProv;
            btcRepository = btcRepo;
            unitOfWork = unitOfW;
        }

        /// <summary>
        /// Fetch and store the price in the local store. 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public async Task<BitCoinPrice> FetchPriceAsync(string source)
        {
            var providerExecutable = btcProvider.FirstOrDefault(fo => fo.Name.ToLower() == source.ToLower());
            if (providerExecutable == null) throw new BTCServiceException($"The Source {source} not found", BTCServiceException.PROVIDER_NOT_FOUND);
            
            var price = await providerExecutable!.GetPriceAsync();
            var result = new BitCoinPrice()
            {
                Price = price,
                Source = source,
                StoreDateTime = DateTime.UtcNow
            };

            await btcRepository.AddAsync(result);
            await unitOfWork.Commit();
            return result;
        }

        public async Task<IList<BitCoinPrice>> GetAllHistoryPrice()
        {
            return (await btcRepository.GetListAsync()).ToList();
        }

        public async Task<IList<BitCoinPrice>> GetHistoryPrice(string source)
        { 
            return (await btcRepository.GetListAsync(wh => wh.Source == source)).ToList();
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
