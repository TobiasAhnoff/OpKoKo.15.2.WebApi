using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpKokoDemo.Models;
using OpKokoDemo.Requests;
using OpKokoDemo.Responses;

namespace OpKokoDemo.Services
{
    public interface IProductService
    {
        Task<GetProductResponse> GetProducts(GetProductRequest request);
        Task<AddProductResponse> AddProducts(AddProductRequest request);
        Task DeleteProduct(DeleteProductRequest request);
    }
}
