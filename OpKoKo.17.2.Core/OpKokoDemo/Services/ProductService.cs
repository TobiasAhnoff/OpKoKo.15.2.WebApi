﻿using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using OpKokoDemo.Config;
using OpKokoDemo.Extensions;
using OpKokoDemo.Models;
using OpKokoDemo.Requests;
using OpKokoDemo.Responses;

namespace OpKokoDemo.Services
{
    public class ProductService : IProductService
    {
        private readonly Language _language;
        private readonly IRepository _repository;
        public ProductService(IRepository repository, IOptions<ProductServiceOptions> options)
        {
            _language = Enum.Parse<Language>(options.Value.Language);
            _repository = repository;
        }

        public async Task<GetProductResponse> GetProducts(int merchantId, string pattern)
        {
            var products = await _repository.GetProducts(merchantId, pattern);
            var response = new GetProductResponse {Products = products};
            return response;
        }

        public async Task<AddProductResponse> AddProducts(int merchantId, AddProductRequest request)
        {
            var product = await _repository.AddProduct(new Product {Language = _language, MerchantId = merchantId, Name = request.Name, Price = request.Price});

            return new AddProductResponse { Product = product};
        }

        public async Task DeleteProduct(int merchantId, int productId)
        {
            await _repository.DeleteProduct(merchantId, productId);
        }
    }
}