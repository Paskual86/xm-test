using Newtonsoft.Json;
using XMCrypto.Domain.Interfaces.Services.Providers;

namespace XMCrypto.Core.Services.Providers.Bitstamp.Dto
{
    public class TickerResponseDto : IBTCTickerDto
    {
        /// <summary>
        /// First Price of the Day
        /// </summary>
        [JsonProperty("open")]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// Last price in counter currency.
        /// </summary>
        [JsonProperty("last")]
        public decimal LastPrice { get; set; }
        /// <summary>
        /// Last 24 hours price high.
        /// </summary>
        [JsonProperty("High")]
        public decimal HighPrice {get; set; }
        /// <summary>
        /// Last 24 hours price low.
        /// </summary>
        [JsonProperty("low")]
        public decimal LowPrice {get; set; }
        /// <summary>
        /// Last 24 hours volume.
        /// </summary>
        [JsonProperty("volume")]
        public decimal Volume { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("vwap")]       
        public decimal VolumeWeightedAveragePrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("ask")]
        
        public decimal BuyPrice { get; set; }
        /// <summary>
        /// Lowest sell order.
        /// </summary>
        [JsonProperty("bid")]
        public decimal SellPrice { get; set; }
        [JsonProperty("timestamp")]
        public string? Timestamp { get; set; }
    }
}
