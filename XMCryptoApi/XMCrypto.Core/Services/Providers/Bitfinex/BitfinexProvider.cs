using Newtonsoft.Json;
using System.Net.NetworkInformation;
using XMCrypto.Core.Services.Providers.Bitfinex.Dto;
using XMCrypto.Core.Utils;
using XMCrypto.Domain.Enums;
using XMCrypto.Domain.Interfaces.Services.Providers;

namespace XMCrypto.Core.Services.Providers.Bitfinex
{
    public class BitfinexProvider : IBTCProviderService
    {
        public const string CLIENT_API_NAME = "BitfinexApi";
        public string Name => "Bitfinex";
        public string UrlProvider { get; private set; }

        private readonly IHttpClientFactory httpClientFactory;

        public BitfinexProvider(IHttpClientFactory httpCF)
        {
            UrlProvider = "v1/pubticker/BTCUSD";
            httpClientFactory = httpCF;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetPriceAsync()
        {
            var response = await GetTickerAsync();
            return response.LastPrice;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ExternalServiceStatus> GetStatusOfServiceAsync()
        {
            var client = httpClientFactory.CreateClient(CLIENT_API_NAME);
            
            if (client == null) 
            {
                // TODO: This should be replace by Internal Exception
                throw new Exception($"The Client {CLIENT_API_NAME} is not configured");
            }

            string url = client.BaseAddress.AbsolutePath;

            var pingResponse =  NetworkUtils.PingService(url);

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
        public async Task<IBTCTickerDto> GetTickerAsync()
        {
            var serviceStatus = await GetStatusOfServiceAsync();
            
            if (serviceStatus != ExternalServiceStatus.Available) 
            {
                throw new Exception("Service not available");
            }

            var client = httpClientFactory.CreateClient(CLIENT_API_NAME);
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
                var result = JsonConvert.DeserializeObject<TickerResponseDto>(responseBody);
                return result;
            }
        }
    }
}
