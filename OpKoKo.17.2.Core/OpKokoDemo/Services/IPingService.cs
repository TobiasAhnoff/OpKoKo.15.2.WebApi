using System;
using System.Threading.Tasks;
using OpKokoDemo.Requests;

namespace OpKokoDemo.Services
{
    public interface IPingService
    {
        Task<Tuple<int, long>> Execute(ExecuteServiceRequest request);
    }
}
