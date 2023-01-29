using AutoMapper;
using Microsoft.OpenApi.Models;
using Serilog;
using XMCrypto.Core.IoC;
using XMCrypto.EntityMapper.Profiles;
using XMCrypto.Persistance.IoC;

namespace XMCryptoApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MX Crypto", Version = "v1" });
            });
            services.ConfigurationDatabase();
            services.ConfigureCore(Configuration);
            services.AddAutoMapper(typeof(MappingDtoProfiles));
            Log.Logger = new LoggerConfiguration()
                            .WriteTo.Console()
                            .CreateLogger();
        }
        

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger) 
        {
            logger.LogInformation("startup-Configure");


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MX Crypto v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
