
using Microsoft.Extensions.Logging;
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
        protected readonly ILogger<BaseProvider<TDto>> logger;
        public string UrlProvider { get; private set; }

        public string Path { get; set; }
        public string Name { get; set; }
        protected string ClientApiName { get; init; }

        public BaseProvider(IHttpClientFactory httpCF, ILogger<BaseProvider<TDto>> log)
        {
            httpClientFactory = httpCF;
            ClientApiName = string.Empty;
            UrlProvider = string.Empty;
            Path = string.Empty;
            Name = string.Empty;
            logger = log;
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
                logger.LogDebug($"There is not configuration for the client: {ClientApiName}");
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
            catch(Exception ex)
            {
                logger.LogDebug($"There was an error cheking the availability of service. Provider: {ClientApiName}. Error: {ex}");
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
                logger.LogDebug($"The service {Name} is not available at this moment");
                throw new BTCProviderException($"The service {Name} is not available at this moment", BTCProviderException.API_SERVICE_NOT_AVAILABLE);
            }
            try
            {
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
                    logger.LogInformation($"Api: {ClientApiName}. Response {responseBody}");
                    return result!;
                }
            }
            catch (Exception ex) 
            {
                logger.LogDebug($"There was an error executing the request. Provider: {ClientApiName}. Error: {ex}");
                throw new BTCProviderException($"There was an error  {Name} is not available at this moment", BTCProviderException.API_SERVICE_NOT_AVAILABLE);
            }
        }        
    }
}
