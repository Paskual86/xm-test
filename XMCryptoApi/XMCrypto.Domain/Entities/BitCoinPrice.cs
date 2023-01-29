namespace XMCrypto.Domain.Entities
{
    public class BitCoinPrice
    {
        public long Id { get; set; }
        public decimal Price { get; set; }
        public string? Source { get; set; }
        public DateTime StoreDateTime { get; set; }
    }
}
