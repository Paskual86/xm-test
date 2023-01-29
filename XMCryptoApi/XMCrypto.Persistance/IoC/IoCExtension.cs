using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using XMCrypto.Domain.Abstractions;
using XMCrypto.Domain.Interfaces.Repository;
using XMCrypto.Persistance.Abstractions;
using XMCrypto.Persistance.Repositories;

namespace XMCrypto.Persistance.IoC
{
    public static class IoCExtension
    {
        public static void ConfigurationDatabase(this IServiceCollection services)
        {
            services.AddScoped<IBTCRepository, BTCRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseInMemoryDatabase("memory_test"));
        }
    }
}