﻿using System;
using System.Diagnostics;
using System.Net.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OpKokoDemo.Services;
using Ploeh.AutoFixture;

namespace OpKokoDemo.ComponentTest
{
    public abstract class ComponentTestBase
    {
        public ComponentTestBase()
        {
            Fixture = new Fixture();
        }
        protected TestServer Server { get; private set; }

        protected HttpClient Client { get; private set; }

        protected Fixture Fixture { get; }

        protected IRepository Repository => (IRepository) Server.Host.Services.GetService(typeof(IRepository));

        protected virtual void Initialize()
        {
        }

        protected virtual Action<IServiceCollection> OverrideConfiguredServices()
        {
            return services => {};
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
            
            Server = TestServerFactory.CreateConfiguredTestServerWithoutJwtBearer("test.testson@omegapoint.se", "all", OverrideConfiguredServices());
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
