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
using OpKokoDemo.Services;
using OpKokoDemo.Validation;
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
            services.AddValidators();

            #region 2 - Trust (JWT)
            //TODO: Config pattern?
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = GetTokenValidationParameters(
                        Configuration["Jwt:SigningCertificateSubjectDistinguishedName"],
                        Configuration["Jwt:Issuer"],
                        Configuration["Jwt:Audience"]);
                });
            #endregion

            // Only allow authenticated users using the JWT Bearer scheme
            var requireBearerAuthenticationPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            services.AddMvc(
                options =>
                {
                    options.Filters.Add(new RequestLoggingFilter());
                    options.Filters.Add(new ExceptionFilter());
                    options.Filters.Add(new ModelStateValidatorFilter());
                    options.Filters.Add(new RequestValidatorFilter());
                    options.Filters.Add(new ResponseLoggingFilter());
                    options.Filters.Add(new AuthorizeFilter(requireBearerAuthenticationPolicy));
                });   
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
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
            
            app.UseAuthentication();

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
