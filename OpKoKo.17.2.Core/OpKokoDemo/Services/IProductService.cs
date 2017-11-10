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
        Task<GetProductResponse> GetProducts(int customerId, string pattern);
        Task<AddProductResponse> AddProduct(int customerId, AddProductRequest request);
        Task DeleteProduct(int customerId, int productId);
    }
}
