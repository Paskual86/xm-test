using System.Text.Json.Serialization;
using XMCrypto.Domain.Interfaces.Services.Providers;

namespace XMCrypto.Core.Services.Providers.Bitstamp.Dto
{
    public class ResponseDto : IBTCTickerDto
    {
        /// <summary>
        /// First Price of the Day
        /// </summary>
        [JsonPropertyName("open")]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// Last price in counter currency.
        /// </summary>
        [JsonPropertyName("last")]
        public decimal LastPrice { get; set; }
        /// <summary>
        /// Last 24 hours price high.
        /// </summary>
        [JsonPropertyName("High")]
        public decimal HighPrice {get; set; }
        /// <summary>
        /// Last 24 hours price low.
        /// </summary>
        [JsonPropertyName("low")]
        public decimal LowPrice {get; set; }
        /// <summary>
        /// Last 24 hours volume.
        /// </summary>
        [JsonPropertyName("volume")]
        public decimal Volume { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("vwap")]       
        public decimal VolumeWeightedAveragePrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("bid")]
        public decimal HighestBuyOrder { get; set; }
        /// <summary>
        /// Lowest sell order.
        /// </summary>
        [JsonPropertyName("ask")]
        public decimal LowestSellOrder { get; set; }
        [JsonPropertyName("timestamp")]
        public string? Timestamp { get; set; }
    }
}
