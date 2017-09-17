using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpKokoDemo.Extensions;
using OpKokoDemo.Filters;
using Serilog;

namespace OpKokoDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddServices(Configuration);
            services.AddRepositories();
            services.SetupLogging();

            services.AddMvc().AddMvcOptions(
                options =>
                {
                    options.Filters.Add(new RequestLoggingFilter());
                    options.Filters.Add(new ExceptionFilter());
                    options.Filters.Add(new ModelStateValidatorFilter());
                    options.Filters.Add(new ResponseLoggingFilter());
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                ////app.UseDeveloperExceptionPage();
            }

            #region CORS
            //#1 - CORS
            //TODO: Get origin white list from config
            //Note that origins URL:s must not end with a "/"
            app.UseCors(
                builder => builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("https://localhost:44304")

            );
            #endregion

            app.UseMvc();
        }
    }
}
