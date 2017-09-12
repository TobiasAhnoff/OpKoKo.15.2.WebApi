using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpKokoDemo.Services
{
    public interface IHttpServiceClient
    {
        Task<Tuple<HttpResponseMessage, long>> GetAsync(string uri);
    }
}