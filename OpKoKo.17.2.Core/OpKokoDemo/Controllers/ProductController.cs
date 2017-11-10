using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpKokoDemo.Requests;
using OpKokoDemo.Services;

namespace OpKokoDemo.Controllers
{
    [Authorize]
    [Route("products")]
    public class ProductController : Controller
    {
        private readonly IProductService _service;
        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpPost("{customerId:int}")]
        [Description("Adds a new product")]
        public async Task<IActionResult> AddProduct([FromRoute]int customerId, [FromBody] AddProductRequest request)
        {
           return new OkObjectResult(await _service.AddProduct(customerId, request));
        }

        [HttpDelete("{customerId:int}/{productId:int}")]
        [Description("Deletes a product")]
        public async Task<IActionResult> RemoveProduct([FromRoute]int customerId, [FromRoute]int productId)
        {
            await _service.DeleteProduct(customerId, productId);
            return new OkResult();
        }

        [HttpGet("{customerId:int}")]
        [Description("Get products gieven a search pattern")]
        public async Task<IActionResult> GetProducts([FromRoute]int customerId, [FromQuery] GetProductRequest request)
        {
            var result = await _service.GetProducts(customerId, request.Pattern);
            return new OkObjectResult(result);
        }
    }
}
