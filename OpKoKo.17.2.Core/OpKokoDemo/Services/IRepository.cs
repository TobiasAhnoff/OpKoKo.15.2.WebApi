﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpKokoDemo.Models;
using OpKokoDemo.Requests;

namespace OpKokoDemo.Services
{
    public interface IRepository
    {
        Task<IEnumerable<Product>> GetProducts(int merchantId, string pattern= null);

        Task<Product> AddProduct(Product produkt);

        Task DeleteProduct(int merchantId, int productId);

    }
}
