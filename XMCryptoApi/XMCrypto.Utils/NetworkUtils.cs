using System.Net.NetworkInformation;

namespace XMCrypto.Utils
{
    public static class NetworkUtils
    {
        public static IPStatus PingService(string url)
        {
            try
            {
                var ping = new Ping();
                var reply = ping.Send(url, 1000);
                return reply.Status;
            }
            catch (PingException)
            {
                throw;
            }
            catch (Exception) 
            {
                throw new Exception("Internal Error");
            }
        }
    }
}
