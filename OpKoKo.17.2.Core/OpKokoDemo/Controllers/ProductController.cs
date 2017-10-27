using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpKokoDemo.Requests;
using OpKokoDemo.Responses;
using OpKokoDemo.Services;

namespace OpKokoDemo.Controllers
{
    [Route("products")]
    public class ProductController : Controller
    {
        private readonly IProductService _service;
        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpPost("{merchantId:int}")]
        public async Task<IActionResult> Post([FromRoute]int merchantId, [FromBody] AddProductRequest request)
        {
            return new OkObjectResult(await _service.AddProducts(merchantId, request));
        }

        [HttpDelete("{merchantId:int}/{productId:int}")]
        public async Task<IActionResult> Delete([FromRoute]int merchantId, [FromRoute]int productId)
        {
            await _service.DeleteProduct(merchantId, productId);
            return new OkResult();
        }

        [HttpGet("{merchantId:int}/")]
        public async Task<IActionResult> Get([FromRoute]int merchantId, [FromQuery] GetProductRequest request)
        {
            await _service.GetProducts(merchantId, request.Pattern);
            return new OkResult();
        }

    }
}
