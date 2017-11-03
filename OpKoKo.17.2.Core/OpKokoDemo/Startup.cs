using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpKokoDemo.Extensions;
using OpKokoDemo.Filters;
using OpKokoDemo.Middleware;
using OpKokoDemo.Services;
using OpKokoDemo.Validation;
using Serilog;

namespace OpKokoDemo
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();


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
            services.AddValidators();

            #region Authentication/Authorization - Require Jwt Bearer scheme
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = Utils.GetTokenValidationParameters(
                        Configuration["Jwt:SigningCertificateSubjectDistinguishedName"],
                        Configuration["Jwt:Issuer"],
                        Configuration["Jwt:Audience"]);
                    options.Events = new JwtBearerEvents {OnAuthenticationFailed = OnAuthenticationFailedHandler};
                });

            var requireJwtBearerAuthorizationPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
            #endregion

            services.AddMvc(
                options =>
                {
                    options.Filters.Add(new RequestLoggingFilter());
                    options.Filters.Add(new ExceptionFilter());
                    options.Filters.Add(new AuthorizeFilter(requireJwtBearerAuthorizationPolicy));
                    options.Filters.Add(new ModelStateValidatorFilter());
                    options.Filters.Add(new RequestValidatorFilter());
                    options.Filters.Add(new ResponseLoggingFilter());
                    
                });   
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseExceptionHandlerMiddleware();

            //Note that origins URL:s must not end with a "/"
            app.UseCors(
                builder => builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins(Configuration["Cors:AllowedOrigins"].Split(',', StringSplitOptions.RemoveEmptyEntries))
            );

            app.UseAuthentication();

            app.UseMvc();
        }

        private Task OnAuthenticationFailedHandler(AuthenticationFailedContext authenticationFailedContext) {
            Log.Logger.Warning(authenticationFailedContext.Exception, "JWT validation error");
            return Task.CompletedTask;
        }
    }
}
