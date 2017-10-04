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

        // POST api/values
        [HttpPost("{merchantId:int}")]
        [Description("Ping uri with a retry limit")]
        public async Task<IActionResult> Post(int merchantId, [FromBody] AddProductRequest request)
        {
            if (request == null)
                return BadRequest();

            return new OkObjectResult(await _service.AddProducts(request));
        }
        
    }
}
