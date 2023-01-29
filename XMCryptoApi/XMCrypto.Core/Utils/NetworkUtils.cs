using System.Net.NetworkInformation;
using XMCrypto.Domain.Enums;

namespace XMCrypto.Core.Utils
{
    public static class NetworkUtils
    {
        public static ExternalServiceStatus PingService(string url) 
        {
            try
            {
                var ping = new Ping();
                var reply = ping.Send(url, 1000);
                if ((reply != null) && (reply.Status == IPStatus.Success))
                {
                    return ExternalServiceStatus.Available;
                }
                else
                {
                    return ExternalServiceStatus.NotAvailable;
                }
            }
            catch
            {
                // TODO: Add Logger the error
                return ExternalServiceStatus.NotAvailable;
            }
        }
    }
}
