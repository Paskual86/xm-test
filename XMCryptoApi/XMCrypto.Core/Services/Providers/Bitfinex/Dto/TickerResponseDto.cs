using Newtonsoft.Json;
using XMCrypto.Domain.Interfaces.Services.Providers;

namespace XMCrypto.Core.Services.Providers.Bitfinex.Dto
{
    public class TickerResponseDto : IBTCTickerDto
    {
        /// <summary>
        /// (bid + ask) / 2
        /// </summary>
        public decimal Mid { get;set;}
        [JsonProperty("bid")]
        public decimal BuyPrice { get;set; }
        [JsonProperty("ask")]
        public decimal SellPrice { get;set; }
        /// <summary>
        /// The price at which the last order executed
        /// </summary>
        [JsonProperty("last_price")]
        public decimal LastPrice { get;set;}
        /// <summary>
        /// Lowest trade price of the last 24 hours
        /// </summary>
        [JsonProperty("low")]
        public decimal LowPrice { get;set;}
        /// <summary>
        /// Highest trade price of the last 24 hours
        /// </summary>
        [JsonProperty("high")]
        public decimal HighPrice { get;set;}
        /// <summary>
        /// Trading volume of the last 24 hours
        /// </summary>
        public decimal Volume { get;set;}

        public string? Timestamp { get;set;}


    }
}
