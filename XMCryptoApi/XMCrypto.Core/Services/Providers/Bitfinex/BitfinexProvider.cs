using Newtonsoft.Json;
using XMCrypto.Core.Services.Providers.Abstractions;
using XMCrypto.Core.Services.Providers.Bitfinex.Dto;
using XMCrypto.Domain.Enums;
using XMCrypto.Domain.Interfaces.Services.Providers;

namespace XMCrypto.Core.Services.Providers.Bitfinex
{
    public class BitfinexProvider : BaseProvider, IBTCProviderService
    {
        public const string CLIENT_API_NAME = "BitfinexApi";
        public string Name => "Bitfinex";
        
        public BitfinexProvider(IHttpClientFactory httpCF): base(httpCF, "v1/pubticker/BTCUSD")
        {
            ClientApiName = CLIENT_API_NAME;
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
