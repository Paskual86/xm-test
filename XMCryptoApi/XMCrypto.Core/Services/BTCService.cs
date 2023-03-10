using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using XMCrypto.Domain.Abstractions;
using XMCrypto.Domain.Entities;
using XMCrypto.Domain.Exceptions;
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
        private readonly ILogger<BTCService> logger;
        public BTCService(IEnumerable<IBTCProviderService> btcProv, IBTCRepository btcRepo, IUnitOfWork unitOfW, ILogger<BTCService> log)
        {
            btcProvider = btcProv;
            btcRepository = btcRepo;
            unitOfWork = unitOfW;
            logger = log;
        }

        /// <summary>
        /// Fetch and store the price in the local store. 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public async Task<BitCoinPrice> FetchPriceAsync(string source)
        {
            var providerExecutable = btcProvider.FirstOrDefault(fo => fo.Name.ToLower() == source.ToLower());
            if (providerExecutable == null) throw new BTCServiceException($"There is not configuration for the client: {source}", BTCServiceException.PROVIDER_NOT_FOUND);

            try
            {
                var price = await providerExecutable!.GetTickerAsync();
                var result = new BitCoinPrice()
                {
                    SellPrice = price.SellPrice,
                    BuyPrice = price.BuyPrice,
                    Source = providerExecutable.Name,
                    StoreDateTime = DateTime.UtcNow
                };

                await btcRepository.AddAsync(result);
                await unitOfWork.Commit();
                return result;
            }
            catch (BTCProviderException btcEx)
            {
                logger.LogDebug(btcEx.ToString());
                throw new BTCServiceException(btcEx.Message, btcEx.ExceptionCode);

            }
            catch (PersistanceException perEx) 
            {
                logger.LogDebug(perEx.ToString());
                if (perEx.ExceptionCode != PersistanceException.COMMIT_ERROR)
                {
                    throw new BTCServiceException(perEx.Message, perEx.ExceptionCode);
                }
                else {
                    throw new BTCServiceException("There was an error saving the information in the store.", BTCServiceException.PROVIDER_NOT_FOUND);
                }
            }
            catch (Exception ex)
            {
                logger.LogDebug(ex.ToString());
                throw new BTCServiceException("Internal Error", BTCServiceException.INTERNAL_ERROR);
            }
        }

        /// <summary>
        /// Get all prices from all souces from the store
        /// </summary>
        /// <returns></returns>
        public async Task<IList<BitCoinPrice>> GetAllHistoryPrice()
        {
            return (await btcRepository.GetListAsync()).ToList();
        }

        public async Task<IList<BitCoinPrice>> GetHistoryPrice(string source)
        { 
            return (await btcRepository.GetListAsync(wh => wh.Source == source)).ToList();
        }

        public async Task<IList<BitCoinPrice>> GetHistoryPrice(string source, DateTime? dateFrom, DateTime? dateTo, int? maxRecords)
        {
            var dtFrom = dateFrom ?? DateTime.UtcNow.AddDays(-5);
            var dtTo = dateTo ?? DateTime.UtcNow.AddDays(5);
            return (await btcRepository.GetListAsync(wh => wh.Source == source && wh.StoreDateTime>= dtFrom && wh.StoreDateTime<= dtTo, null, maxRecords)).ToList();
        }

        public async Task<IList<CryptoProvider>> GetSourceAvailablesAsync()
        {
            // First  - Load all implementations of IBTCProviderService
            // Second - Loop the values and check out if the service is available
            // Third  - Add The Valid Url to the result
            var result = new List<CryptoProvider>();

            if ((btcProvider == null) || !btcProvider.Any() ) {
                throw new BTCServiceException("There are not a providers implementations", BTCServiceException.NO_PROVIDERS_IMPLEMENTATION);
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
                    logger.LogInformation($"Provider {provider.Name}, the service is not available");
                }
            }
            return await Task.FromResult(result);
        }
    }
}
