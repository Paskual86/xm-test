using System.Text.Json.Serialization;
using XMCrypto.Domain.Interfaces.Services.Providers;

namespace XMCrypto.Core.Services.Providers.Bitfinex.Dto
{
    public class ResponseDto : IBTCTickerDto
    {
        /// <summary>
        /// (bid + ask) / 2
        /// </summary>
        public decimal Mid { get;set;}
        [JsonPropertyName("bid")]
        public decimal HighestBuyOrder { get;set;}
        [JsonPropertyName("ask")]
        public decimal LowestSellOrder { get;set;}
        /// <summary>
        /// The price at which the last order executed
        /// </summary>
        [JsonPropertyName("last_price")]
        public decimal LastPrice { get;set;}
        /// <summary>
        /// Lowest trade price of the last 24 hours
        /// </summary>
        [JsonPropertyName("low")]
        public decimal LowPrice { get;set;}
        /// <summary>
        /// Highest trade price of the last 24 hours
        /// </summary>
        [JsonPropertyName("high")]
        public decimal HighPrice { get;set;}
        /// <summary>
        /// Trading volume of the last 24 hours
        /// </summary>
        public decimal Volume { get;set;}

        public TimeSpan Timestamp { get;set;}


    }
}
