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
        Task<GetProductResponse> GetProducts(int merchantId, string pattern);
        Task<AddProductResponse> AddProducts(int merchantId, AddProductRequest request);
        Task DeleteProduct(int merchantId, int productId);
    }
}
