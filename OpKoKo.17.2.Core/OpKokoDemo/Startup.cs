using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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

            #region Authorization
            //Require Beare scheme and trust JWT
            ////services.AddAuthentication(options =>
            ////    {
            ////        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            ////        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            ////    })
            ////    .AddJwtBearer(options =>
            ////    {
            ////        options.TokenValidationParameters = GetTokenValidationParameters(
            ////            Configuration["Jwt:SigningCertificateSubjectDistinguishedName"],
            ////            Configuration["Jwt:Issuer"],
            ////            Configuration["Jwt:Audience"]);
            ////    });
            

            // Only allow authenticated users using the JWT Bearer scheme
            var requireBearerAuthenticationPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
            #endregion

            services.AddMvc(
                options =>
                {
                    options.Filters.Add(new RequestLoggingFilter());
                    ////options.Filters.Add(new AuthorizeFilter(requireAuthenticatedUserPolicy));
                    options.Filters.Add(new ExceptionFilter());
                    options.Filters.Add(new ModelStateValidatorFilter());
                    options.Filters.Add(new RequestValidatorFilter());
                    options.Filters.Add(new ResponseLoggingFilter());
                    
                });   
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.UseExceptionHandlerMiddleware();
                });
            }

            //Note that origins URL:s must not end with a "/"
            app.UseCors(
                builder => builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("https://localhost:44304")

            );

            //Authentication
            var useDebugWithNoAuthentication = false;
#if DEBUG
            useDebugWithNoAuthentication = Configuration.GetValue<bool>("AppSettings:UseDebugWithNoAuthentication");
#endif

            if (useDebugWithNoAuthentication && env.IsDevelopment())
            {
                app.UseDebugWithNoAuthentication(
                    Configuration.GetValue<string>("AppSettings:DeveloperSub"),
                    Configuration.GetValue<string>("AppSettings:DeveloperScope"));
            }
            else
            {
                app.UseAuthentication();
            }

            app.UseMvc();
        }

#region 2 - Trust (JWT)
        private static TokenValidationParameters GetTokenValidationParameters(
            string signingCertificateSubjectDistinguishedName, string issuer, string audience) {
            var signingCert = Utils.GetCertFromCertStore(signingCertificateSubjectDistinguishedName);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new X509SecurityKey(signingCert),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(5)
            };
            return tokenValidationParameters;
        }
#endregion
    }
}
