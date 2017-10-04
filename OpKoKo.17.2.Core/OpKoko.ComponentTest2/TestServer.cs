using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace OpKokoDemo.ComponentTest
{
    //public class TestServer : IServer, IDisposable
    //{
    //    private const string ServerName = "TestServer";
    //    private IWebHost _hostInstance;
    //    private bool _disposed;
    //    private IHttpApplication<HostingApplication.Context> _application;

    //    public TestServer(IWebHostBuilder builder)
    //      : this(builder, (IFeatureCollection)new FeatureCollection())
    //    {
    //    }

    //    public TestServer(IWebHostBuilder builder, IFeatureCollection featureCollection)
    //    {
    //        if (builder == null)
    //            throw new ArgumentNullException(nameof(builder));
    //        if (featureCollection == null)
    //            throw new ArgumentNullException(nameof(featureCollection));
    //        this.Features = featureCollection;
    //        IWebHost webHost = builder.UseServer((IServer)this).Build();
    //        webHost.StartAsync(new CancellationToken()).GetAwaiter().GetResult();
    //        this._hostInstance = webHost;
    //    }

    //    public Uri BaseAddress { get; set; } = new Uri("http://localhost/");

    //    public IWebHost Host
    //    {
    //        get
    //        {
    //            return this._hostInstance;
    //        }
    //    }

    //    public IFeatureCollection Features { get; }

    //    public HttpMessageHandler CreateHandler()
    //    {
    //        return (HttpMessageHandler)new ClientHandler(this.BaseAddress == (Uri)null ? PathString.Empty : PathString.FromUriComponent(this.BaseAddress), this._application);
    //    }

    //    public HttpClient CreateClient()
    //    {
    //        return new HttpClient(this.CreateHandler())
    //        {
    //            BaseAddress = this.BaseAddress
    //        };
    //    }

    //    public WebSocketClient CreateWebSocketClient()
    //    {
    //        return new WebSocketClient(this.BaseAddress == (Uri)null ? PathString.Empty : PathString.FromUriComponent(this.BaseAddress), this._application);
    //    }

    //    /// <summary>Begins constructing a request message for submission.</summary>
    //    /// <param name="path"></param>
    //    /// <returns><see cref="T:Microsoft.AspNetCore.TestHost.RequestBuilder" /> to use in constructing additional request details.</returns>
    //    public RequestBuilder CreateRequest(string path)
    //    {
    //        return new RequestBuilder(this, path);
    //    }

    //    public void Dispose()
    //    {
    //        if (this._disposed)
    //            return;
    //        this._disposed = true;
    //        this._hostInstance.Dispose();
    //    }

    //    Task IServer.StartAsync<TContext>(IHttpApplication<TContext> application, CancellationToken cancellationToken)
    //    {
    //        this._application = (IHttpApplication<HostingApplication.Context>)new TestServer.ApplicationWrapper<HostingApplication.Context>((IHttpApplication<HostingApplication.Context>)application, (Action)(() =>
    //        {
    //            if (this._disposed)
    //                throw new ObjectDisposedException(this.GetType().FullName);
    //        }));
    //        return Task.CompletedTask;
    //    }

    //    Task IServer.StopAsync(CancellationToken cancellationToken)
    //    {
    //        return Task.CompletedTask;
    //    }

    //    private class ApplicationWrapper<TContext> : IHttpApplication<TContext>
    //    {
    //        private readonly IHttpApplication<TContext> _application;
    //        private readonly Action _preProcessRequestAsync;

    //        public ApplicationWrapper(IHttpApplication<TContext> application, Action preProcessRequestAsync)
    //        {
    //            this._application = application;
    //            this._preProcessRequestAsync = preProcessRequestAsync;
    //        }

    //        public TContext CreateContext(IFeatureCollection contextFeatures)
    //        {
    //            return this._application.CreateContext(contextFeatures);
    //        }

    //        public void DisposeContext(TContext context, Exception exception)
    //        {
    //            this._application.DisposeContext(context, exception);
    //        }

    //        public Task ProcessRequestAsync(TContext context)
    //        {
    //            this._preProcessRequestAsync();
    //            return this._application.ProcessRequestAsync(context);
    //        }
    //    }
    //}
}
