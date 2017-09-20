using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpKokoDemo.Config;
using OpKokoDemo.Services;
using Serilog;

namespace OpKokoDemo.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtServiceOptions>(configuration.GetSection("Jwt"));
            services.Configure<PingServiceConfig>(configuration.GetSection("PingService"));
            services.Configure<ProductServiceOptions>(configuration.GetSection("ProductService"));

            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<IPingService, PingService>();
            services.AddSingleton<ITokenService, TestTokenService>();
            services.AddSingleton<IHttpServiceClient, HttpServiceClient>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IRepository, Repository>();
        }

        public static void SetupLogging(this IServiceCollection services)
        {
            services.AddSingleton(provider => Log.Logger);

            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));
        }
    }
}
