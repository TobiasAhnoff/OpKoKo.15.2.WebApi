﻿using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using OpKokoDemo.Extensions;
using OpKokoDemo.Filters;
using OpKokoDemo.Middleware;
using OpKokoDemo.Validation;

namespace OpKokoDemo.ComponentTest
{
    public static class TestServerFactory
    {
        private const string SolutionName = "OpKokoDemo.sln";
        private const string WebApiSourcePath = "";

        public static TestServer CreateConfiguredTestServerWithoutJwtBearer(string testSub, string testScope, Action<IServiceCollection> testSpecificServiceConfiguration = null)
        {
            var startupAssembly = typeof(Startup).GetTypeInfo().Assembly;
            var path = GetProjectPath(startupAssembly);

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json");
            var config = configBuilder.Build();

            var builder = new WebHostBuilder()
                .ConfigureServices(
                    services =>
                    {
                        OverrideMvcDefaultControllerLocationProvider(services);

                        services.AddOptions();
                        services.AddServices(config);
                        services.AddRepositories();
                        services.SetupLogging();
                        services.AddValidators();

                        #region Authentication/Authorization - Require Jwt Bearer scheme
                        //services.AddAuthentication(options =>
                        //    {
                        //        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        //        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        //    })
                        //    .AddJwtBearer(options =>
                        //    {
                        //        options.TokenValidationParameters = Utils.GetTokenValidationParameters(
                        //            config["Jwt:SigningCertificateSubjectDistinguishedName"],
                        //            config["Jwt:Issuer"],
                        //            config["Jwt:Audience"]);
                        //        //options.Events = new JwtBearerEvents { OnAuthenticationFailed = OnAuthenticationFailedHandler };
                        //    });

                        //var requireJwtBearerAuthorizationPolicy = new AuthorizationPolicyBuilder()
                        //    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                        //    .RequireAuthenticatedUser()
                        //    .Build();
                        #endregion

                        services.AddMvc(options =>
                        {
                            options.Filters.Add(new RequestLoggingFilter());
                            options.Filters.Add(new ExceptionFilter());
                            //options.Filters.Add(new AuthorizeFilter(requireJwtBearerAuthorizationPolicy));
                            options.Filters.Add(new ModelStateValidatorFilter());
                            options.Filters.Add(new RequestValidatorFilter());
                            options.Filters.Add(new ResponseLoggingFilter());
                        });


                        testSpecificServiceConfiguration?.Invoke(services);
                    })
                .UseContentRoot(GetProjectPath(startupAssembly))
                .UseEnvironment("Development")
                .Configure(
                    app =>
                    {
                        app.UseExceptionHandlerMiddleware();

                        //Note that origins URL:s must not end with a "/"
                        //app.UseCors(
                        //    builder => builder
                        //        .AllowAnyHeader()
                        //        .AllowAnyMethod()
                        //        .WithOrigins(config["Cors:AllowedOrigins"].Split(',', StringSplitOptions.RemoveEmptyEntries))
                        //);

                        app.UseTestNoAuthMiddleware(testSub, testScope);
                        //app.UseAuthentication();

                        app.UseMvc();
                    });

            return new TestServer(builder);
        }

        private static void OverrideMvcDefaultControllerLocationProvider(IServiceCollection services)
        {
            var startupAssembly = typeof(Startup).GetTypeInfo().Assembly;

            // Inject a custom application part manager. Overrides AddMvcCore() because that uses TryAdd().
            var manager = new ApplicationPartManager();
            manager.ApplicationParts.Add(new AssemblyPart(startupAssembly));

            manager.FeatureProviders.Add(new ControllerFeatureProvider());
            manager.FeatureProviders.Add(new ViewComponentFeatureProvider());

            services.AddSingleton(manager);
        }

        /// <summary>
        /// Gets the full path to the target project path that we wish to test
        /// </summary>
        /// <param name="startupAssembly">The target project's assembly.</param>
        /// <returns>The full path to the target project.</returns>
        private static string GetProjectPath(Assembly startupAssembly)
        {
            // Get name of the target project which we want to test
            var projectName = startupAssembly.GetName().Name;

            // Get currently executing test project path
            var applicationBasePath = PlatformServices.Default.Application.ApplicationBasePath;

            // Find the folder which contains the solution file. We then use this information to find the target
            // project which we want to test.
            var directoryInfo = new DirectoryInfo(applicationBasePath);
            do
            {
                var solutionFileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, SolutionName));
                if (solutionFileInfo.Exists)
                {
                    return Path.GetFullPath(Path.Combine(directoryInfo.FullName, WebApiSourcePath, projectName));
                }

                directoryInfo = directoryInfo.Parent;
            }
            while (directoryInfo?.Parent != null);

            throw new Exception($"Solution root could not be located using application root {applicationBasePath}.");
        }
    }
}
