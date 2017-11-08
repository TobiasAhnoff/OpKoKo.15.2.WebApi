using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpKokoDemo.Models;
using OpKokoDemo.Requests;

namespace OpKokoDemo.Services
{
    public class Repository : IRepository
    {
        private List<Product> _products;

        public Repository()
        {
            SeedProducts();
        }

        public Task<IEnumerable<Product>> GetProducts(int customerId, string pattern = null)
        {
            return Task.FromResult(_products.Where(p => (customerId == -1 || p.CustomerId == customerId)  && (string.IsNullOrEmpty(pattern) || p.Name.Contains(pattern))));
        }

        public Task<Product> AddProduct(Product product)
        {
            var customerExists = _products.Any(p => p.CustomerId == product.CustomerId);
            if (!customerExists)
            {
                throw new ArgumentException($"Customer with id {product.CustomerId} does not exist");
            }
            product.Id = _products.OrderByDescending(p => p.Id).First().Id + 1;
            _products.Add(product);

            return Task.FromResult(product);
        }

        public Task DeleteProduct(int customerId, int productId)
        {
            var product = _products.FirstOrDefault(p => p.CustomerId == customerId && p.Id == productId);
            if (product == null)
            {
                throw new ArgumentException($"Product with customer id {customerId} and product id {productId} does not exist");
            }
            _products.Remove(product);
            return Task.FromResult(product);
        }

        private void SeedProducts()
        {
            _products = new List<Product>();
            for (int i = 0; i < 100; i++)
            {
                _products.Add(new Product(GetCustomer(i), $"Artikel {i + 1}", new Random().Next(1, 9999), GetLangugage(i)){Id = i});
            }
        }

        private Language GetLangugage(int index)
        {
            if (index < 33)
            {
                return Language.SE;
            }
            if (index < 66)
            {
                return Language.DK;
            }
            return Language.NO;
        }

        private int GetCustomer(int index)
        {
            return index % 10;
        }
    }
}