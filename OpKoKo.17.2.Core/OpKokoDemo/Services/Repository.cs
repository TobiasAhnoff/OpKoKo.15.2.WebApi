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

        public Task<IEnumerable<Product>> GetProducts(int merchantId, Language language)
        {
            return Task.FromResult(_products.Where(p => p.MerchantId == merchantId && p.Language == language));
        }

        public Task<Product> AddProduct(Product product)
        {
            product.Id = _products.OrderByDescending(p => p.Id).First().Id + 1;
            _products.Add(product);

            return Task.FromResult(product);
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
                return Language.SE;
            }
            if (index < 66)
            {
                return Language.DK;
            }
            return Language.NO;
        }

        private int GetMerchant(int index)
        {
            return index / 10;
        }
    }
}