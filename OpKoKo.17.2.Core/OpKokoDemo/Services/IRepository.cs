using System.Collections.Generic;
using System.Threading.Tasks;
using OpKokoDemo.Models;

namespace OpKokoDemo.Services
{
    public interface IRepository
    {
        Task<IEnumerable<Product>> GetProducts(CustomerId customerId, string pattern= null);

        Task<Product> AddProduct(Product produkt);

        Task DeleteProduct(CustomerId customerId, ProductId productId);
    }
}
