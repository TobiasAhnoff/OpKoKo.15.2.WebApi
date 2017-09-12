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

        public Task<Product> GetProduct(int merchantId, int id)
        {
            return Task.FromResult(_products.First(p => p.MerchantId == merchantId && p.Id == id));
        }

        public Task<IEnumerable<Product>> GetProducts(int merchantId)
        {
            return Task.FromResult(_products.Where(p => p.MerchantId == merchantId));
        }

        public Task<Product> AddProduct(AddProductRequest request)
        {
            var produkt = new Product
            {
                Id = _products.Count + 1,
                Language = (Language) Enum.Parse(typeof(Language), request.Language, false),
                MerchantId = request.MerchantId,
                Name = request.Name,
                Price = request.Price
            };
            _products.Add(produkt);

            return Task.FromResult(produkt);
        }

        public Task DeleteProduct(int merchantId, int id)
        {
            var product = _products.First(p => p.MerchantId == merchantId && p.Id == id);
            _products.Remove(product);
            return Task.FromResult(product);
        }

        private void SeedProducts()
        {
            _products = new List<Product>();
            for (int i = 0; i < 100; i++)
            {
                _products.Add(new Product
                {
                    Id = i,
                    Language = GetLangugage(i),
                    MerchantId = GetMerchant(i),
                    Name = $"Artikel {i + 1}",
                    Price = new Random().Next(1, 9999)
                });
            }
        }

        private Language GetLangugage(int index)
        {
            if (index < 33)
            {
                return Language.Se;
            }
            if (index < 66)
            {
                return Language.Dk;
            }
            return Language.No;
        }

        private int GetMerchant(int index)
        {
            return index / 10;
        }
    }
}