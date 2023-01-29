
using Newtonsoft.Json;
using XMCrypto.Domain.Enums;
using XMCrypto.Domain.Exceptions;
using XMCrypto.Domain.Interfaces.Services.Providers;

namespace XMCrypto.Core.Services.Providers.Abstractions
{
    public abstract class BaseProvider<TDto>
        where TDto: class, IBTCTickerDto
    {
        protected readonly IHttpClientFactory httpClientFactory;

        public string UrlProvider { get; private set; }

        public string Path { get; set; }
        public string Name { get; set; }
        protected string ClientApiName { get; init; }

        public BaseProvider(IHttpClientFactory httpCF)
        {
            httpClientFactory = httpCF;
            ClientApiName = string.Empty;
            UrlProvider = string.Empty;
            Path = string.Empty;
            Name = string.Empty;
        }

        /// <summary>
        /// Checkout if the service is available
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public virtual async Task<ExternalServiceStatus> GetStatusOfServiceAsync()
        {
            var client = httpClientFactory.CreateClient(ClientApiName);

            if (client == null)
            {
                throw new BTCProviderException($"There is not configuration for the client: {ClientApiName}", BTCProviderException.CLIENT_API_NOT_CONFIGURED_CODE);
            }

            try
            {
                using (var response = await client.GetAsync(""))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        UrlProvider = client.BaseAddress + Path;
                        return ExternalServiceStatus.Available;
                    }
                    else
                        return ExternalServiceStatus.NotAvailable;
                }
            }
            catch
            {
                return ExternalServiceStatus.NotAvailable;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public virtual async Task<TDto> GetCommonTickerAsync() 
        {
            var serviceStatus = await GetStatusOfServiceAsync();

            if (serviceStatus != ExternalServiceStatus.Available)
            {
                throw new BTCProviderException($"The service {Name} is not available at this moment", BTCProviderException.API_SERVICE_NOT_AVAILABLE);
            }

            var client = httpClientFactory.CreateClient(ClientApiName);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(client.BaseAddress + Path),
                Headers =  {
                                { "accept", "application/json" },
                            },
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TDto>(responseBody);
                return result!;
            }
        }

        /// <summary>
        /// Return the last price of BTC returned by the Service
        /// </summary>
        /// <returns>decimal</returns>
        public virtual async Task<decimal> GetPriceAsync()
        {
            var response = await GetCommonTickerAsync();
            return response.LastPrice;
        }
    }
}
