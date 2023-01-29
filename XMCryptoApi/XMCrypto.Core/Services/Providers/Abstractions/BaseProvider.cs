
using Newtonsoft.Json;
using XMCrypto.Core.Utils;
using XMCrypto.Domain.Enums;
using XMCrypto.Domain.Interfaces.Services.Providers;

namespace XMCrypto.Core.Services.Providers.Abstractions
{
    public abstract class BaseProvider<TDto>
        where TDto: class, IBTCTickerDto
    {
        protected readonly IHttpClientFactory httpClientFactory;

        public string UrlProvider { get; private set; }

        protected string ClientApiName { get; init; }

        public BaseProvider(IHttpClientFactory httpCF, string urlProvider)
        {
            httpClientFactory = httpCF;
            UrlProvider = urlProvider;
            ClientApiName = string.Empty;
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

            string url = client.BaseAddress!.AbsolutePath;

            var pingResponse = NetworkUtils.PingService(url);

            using (var response = await client.GetAsync(""))
            {
                if ((pingResponse == ExternalServiceStatus.Available) && (response.StatusCode == System.Net.HttpStatusCode.OK))
                    return ExternalServiceStatus.Available;
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
                RequestUri = new Uri(UrlProvider),
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
