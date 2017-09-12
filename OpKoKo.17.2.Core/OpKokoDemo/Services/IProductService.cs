using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpKokoDemo.Models;
using OpKokoDemo.Responses;

namespace OpKokoDemo.Services
{
    public interface IProductService
    {
        Task<GetProductResponse> GetProducts();
        Task<AddProductResponse> AddProducts();
        Task<DeleteProductResponse> DeleteProduct();
    }
}
