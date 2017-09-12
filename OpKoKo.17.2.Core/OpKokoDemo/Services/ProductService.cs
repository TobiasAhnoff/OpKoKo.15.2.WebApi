using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using OpKokoDemo.Config;
using OpKokoDemo.Responses;

namespace OpKokoDemo.Services
{
    class ProductService : IProductService
    {
        private ProductServiceOptions _options;
        private IRepository _repository;
        public ProductService(IRepository repository, IOptions<ProductServiceOptions> options)
        {
            _options = options.Value;
            _repository = repository;
        }

        public Task<GetProductResponse> GetProducts()
        {
            throw new NotImplementedException();
        }

        public Task<AddProductResponse> AddProducts()
        {
            throw new NotImplementedException();
        }

        public Task<DeleteProductResponse> DeleteProduct()
        {
            throw new NotImplementedException();
        }
    }
}