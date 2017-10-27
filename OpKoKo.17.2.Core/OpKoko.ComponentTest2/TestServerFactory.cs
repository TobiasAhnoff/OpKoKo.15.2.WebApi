using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using OpKokoDemo.Extensions;
using OpKokoDemo.Filters;
using OpKokoDemo.Validation;

namespace OpKokoDemo.ComponentTest
{
    public static class TestServerFactory
    {
        private const string SolutionName = "OpKokoDemo.sln";
        private const string WebApiSourcePath = "";

        public static TestServer CreateConfiguredTestServer(Action<IServiceCollection> testSpecificServiceConfiguration = null)
        {
            var startupAssembly = typeof(Startup).GetTypeInfo().Assembly;
            var path = GetProjectPath(startupAssembly);

            var builder2 = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json");
            var config = builder2.Build();

            var builder = new WebHostBuilder()
                .ConfigureServices(
                    services =>
                    {
                        services.AddMvc(options =>
                        {
                            options.Filters.Add(new ExceptionFilter());
                            options.Filters.Add(new ModelStateValidatorFilter());
                            options.Filters.Add(new RequestValidatorFilter());

                            ////options.Filters.Add(new AuthorizeFilter(requireBearerAuthenticationPolicy));

                        });
                        OverrideMvcDefaultControllerLocationProvider(services);

                        services.AddOptions();
                        services.AddServices(config);
                        services.AddRepositories();
                        services.SetupLogging();
                        services.AddValidators();


                        testSpecificServiceConfiguration?.Invoke(services);
                    })
                .UseContentRoot(GetProjectPath(startupAssembly))
                .UseEnvironment("Development")
                .Configure(
                    app =>
                    {
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
