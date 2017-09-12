using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpKokoDemo.Services
{
    class HttpServiceClient : IHttpServiceClient
    {
        public async Task<Tuple<HttpResponseMessage, long>> GetAsync(string uri)
        {
            var httpClient = new HttpClient();
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var res = await httpClient.GetAsync(uri);
            stopWatch.Stop();
            return new Tuple<HttpResponseMessage, long>(res, stopWatch.ElapsedMilliseconds);
        }
    }
}