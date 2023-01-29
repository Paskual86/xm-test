namespace XMCrypto.Domain.Interfaces.Services.Providers
{
    public interface IBTCTickerDto
    {
        decimal LastPrice { get; set; }
        decimal LowPrice { get; set; }
        decimal HighPrice { get; set; }
        decimal BuyPrice { get; set; }
        decimal SellPrice { get; set; }
        decimal Volume { get; set; }
    }
}
