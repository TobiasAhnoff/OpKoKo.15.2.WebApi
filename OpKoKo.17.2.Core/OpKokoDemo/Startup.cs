using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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

            #region 2 - Trust (JWT)
            //TODO: Config pattern?
            services.AddAuthentication(options => options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme)
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
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();

            services.AddMvc().AddMvcOptions(
                options =>
                {
                    options.Filters.Add(new RequestLoggingFilter());
                    options.Filters.Add(new ExceptionFilter());
                    options.Filters.Add(new ModelStateValidatorFilter());
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
            
            //TODO: not needed?
            //app.UseAuthentication();

            app.UseMvc();
        }

        #region 2 - Trust (JWT)
        private static TokenValidationParameters GetTokenValidationParameters(
            string signingCertificateSubjectDistinguishedName, string issuer, string audience) {
            var signingCert = GetSigningCertifacate(signingCertificateSubjectDistinguishedName);

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

        public static X509Certificate2 GetSigningCertifacate(string signingCertificateSubjectDistinguishedName) {
            //TODO
            //Azure uses the cert store of the current user and there is no easy way to add a self-signed cert to trusted people...
            //thus validOnly needs to be set to false when unsing a self-signed cert.
            //https://azure.microsoft.com/en-us/blog/using-certificates-in-azure-websites-applications/
            //Maybe somthing like this could solve the problem
            //http://leastprivilege.com/2011/03/01/adding-a-certificate-to-the-root-certificate-store-from-the-command-line-e-g-as-an-azure-startup-task/

            //Create signign cert with
            //https://brockallen.com/2015/06/01/makecert-and-creating-ssl-or-signing-certificates/

            //var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            var certificate = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, signingCertificateSubjectDistinguishedName, false)[0];

            return certificate;
        }
        #endregion
    }
}
