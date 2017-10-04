using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace OpKokoDemo.ComponentTest
{
    public abstract class ComponentTest
    {
        protected TestServer Server { get; private set; }

        protected HttpClient Client { get; private set; }

        protected Fixture Fixture { get; }

        protected virtual void Initialize()
        {
        }

        protected virtual Action<IServiceCollection> OverrideConfiguredServices()
        {
            return services => { };
        }

        protected abstract void Setup();

        protected abstract void Act();

        protected virtual void AssertionPreparation()
        {
        }

        protected virtual void TearDown()
        {
        }

        [OneTimeSetUp]
        public void TestInitializer()
        {
            Initialize();

            Server = TestServerFactory.CreateConfiguredTestServer(OverrideConfiguredServices());
            Client = Server.CreateClient();

            Trace.WriteLine("Running setup...");
            Setup();

            Trace.WriteLine("Running act...");
            Act();

            Trace.WriteLine("Running assertion preparation...");
            AssertionPreparation();
        }

        [OneTimeTearDown]
        public void TestFinalizer()
        {
            Client.Dispose();
            Server.Dispose();

            TearDown();
        }


    }
}
