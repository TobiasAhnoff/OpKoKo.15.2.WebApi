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
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _service;
        public ProductController(IProductService service)
        {
            _service = service;
        }

        // POST api/values
        [HttpPost("{id:int}/execute")]
        [Description("Ping uri with a retry limit")]
        public async Task<IActionResult> Post(int merchantId, [FromBody] AddProductRequest request)
        {
            if (request == null)
                return BadRequest();

            var res = await _service.AddProducts();
            return null;
            ////return new OkObjectResult(new ExecuteServiceResponse { Counter = res, ElapsedMilliseconds = res.Item2 });
        }

    }
}
