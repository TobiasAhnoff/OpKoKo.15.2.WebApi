using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OpKokoDemo.Config;
using OpKokoDemo.Services;
using OpKokoDemo.Validation;
using Serilog;

namespace OpKokoDemo.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static readonly Assembly ValidatorAssembly = typeof(GetProductRequestValidator).Assembly;

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

        public static void AddValidators(this IServiceCollection services)
        {
            var exportedTypes = ValidatorAssembly.GetExportedTypes();
            var requestValidatorTypes = exportedTypes.Where(t => typeof(IRequestValidator).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var requestValidatorType in requestValidatorTypes)
            {
                services.TryAddEnumerable(ServiceDescriptor.Transient(typeof(IRequestValidator), requestValidatorType));
            }
        }
    }
}
