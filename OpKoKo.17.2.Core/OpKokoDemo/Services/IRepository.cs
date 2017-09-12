using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpKokoDemo.Models;
using OpKokoDemo.Requests;

namespace OpKokoDemo.Services
{
    public interface IRepository
    {
        Task<Product> GetProduct(int merchantId, int id);

        Task<IEnumerable<Product>> GetProducts(int merchantId);

        Task<Product> AddProduct(AddProductRequest produkt);

        Task DeleteProduct(int merchantId, int id);

    }
}
