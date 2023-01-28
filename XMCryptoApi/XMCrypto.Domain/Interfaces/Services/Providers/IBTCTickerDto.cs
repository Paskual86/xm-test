namespace XMCrypto.Domain.Interfaces.Services.Providers
{
    public interface IBTCTickerDto
    {
        decimal LastPrice { get; set; }
        decimal LowPrice { get; set; }
        decimal HighPrice { get; set; }
        decimal HighestBuyOrder { get; set; }
        decimal LowestSellOrder { get; set; }
        decimal Volume { get; set; }
    }
}
