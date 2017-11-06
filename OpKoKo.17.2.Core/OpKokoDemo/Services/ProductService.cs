using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OpKokoDemo.Config;
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
            var response = new GetProductResponse { Products = products };
            return response;
        }

        public async Task<AddProductResponse> AddProducts(int merchantId, AddProductRequest request)
        {
            var productModel = new Product(merchantId, request.Name, request.Price, _language);
            var product = await _repository.AddProduct(productModel);

            return new AddProductResponse { Product = product };
        }

        public async Task DeleteProduct(int merchantId, int productId)
        {
            await _repository.DeleteProduct(merchantId, productId);
        }
    }
}