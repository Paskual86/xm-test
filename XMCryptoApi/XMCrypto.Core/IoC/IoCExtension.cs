using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XMCrypto.Core.Services.Providers.Bitfinex;
using XMCrypto.Core.Services.Providers.Bitstamp;

namespace XMCrypto.Core.IoC
{
    public static class IoCExtension
    {
        public static void ConfigureCore(this IServiceCollection services, IConfiguration configuration) 
        {

            services.AddHttpClient(BitfinexProvider.CLIENT_API_NAME, httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration.GetSection("BTCProviders").GetValue<string>(BitfinexProvider.CLIENT_API_NAME+"Url") + "/");
            });

            services.AddHttpClient(BitstampProvider.CLIENT_API_NAME, httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration.GetSection("BTCProviders").GetValue<string>(BitfinexProvider.CLIENT_API_NAME + "Url") + "/");
            });
        }
    }
}
