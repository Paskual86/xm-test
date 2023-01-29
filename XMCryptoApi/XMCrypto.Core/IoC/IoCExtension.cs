using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XMCrypto.Core.Services;
using XMCrypto.Core.Services.Providers.Bitfinex;
using XMCrypto.Core.Services.Providers.Bitstamp;
using XMCrypto.Domain.Interfaces.Services;
using XMCrypto.Domain.Interfaces.Services.Providers;

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

            services.ConfigureProviders();
            services.AddScoped<IBTCService, BTCService>();
        }

        public static void ConfigureProviders(this IServiceCollection services) 
        {
            //services.AddScoped<IBTCProviderService<IBTCTickerDto>, BaseProvider<IBTCTickerDto>>();
            services.AddScoped<IBTCProviderService<Services.Providers.Bitfinex.Dto.TickerResponseDto>, BitfinexProvider>();
            services.AddScoped<IBTCProviderService<Services.Providers.Bitstamp.Dto.TickerResponseDto>, BitstampProvider>();
        }
    }
}
