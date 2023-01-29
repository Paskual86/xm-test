
using Newtonsoft.Json;
using XMCrypto.Domain.Enums;
using XMCrypto.Domain.Interfaces.Services.Providers;
using XMCrypto.Utils;

namespace XMCrypto.Core.Services.Providers.Abstractions
{
    public abstract class BaseProvider<TDto>
        where TDto: class, IBTCTickerDto
    {
        protected readonly IHttpClientFactory httpClientFactory;

        public string UrlProvider { get; private set; }

        public string Path { get; set; }
        protected string ClientApiName { get; init; }

        public BaseProvider(IHttpClientFactory httpCF)
        {
            httpClientFactory = httpCF;
            ClientApiName = string.Empty;
            UrlProvider = string.Empty;
            Path = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public virtual async Task<ExternalServiceStatus> GetStatusOfServiceAsync()
        {
            var client = httpClientFactory.CreateClient(ClientApiName);

            if (client == null)
            {
                // TODO: This should be replace by Internal Exception
                throw new Exception($"The Client {ClientApiName} is not configured");
            }
            
            var pingResponse = NetworkUtils.PingService(client.BaseAddress!.Host);

            using (var response = await client.GetAsync(""))
            {
                if ((pingResponse == System.Net.NetworkInformation.IPStatus.Success) && (response.StatusCode == System.Net.HttpStatusCode.OK))
                {
                    UrlProvider = client.BaseAddress + Path;
                    return ExternalServiceStatus.Available;
                }
                else
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
                throw new Exception("Service not available");
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
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual async Task<decimal> GetPriceAsync()
        {
            var response = await GetCommonTickerAsync();
            return response.LastPrice;
        }
    }
}
