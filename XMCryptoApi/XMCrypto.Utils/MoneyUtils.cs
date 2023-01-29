namespace XMCrypto.Utils
{
    public static class MoneyUtils
    {
        public static string FormatPrice(this decimal value)
        {
            return value.ToString("0.00");
        }
    }
}
