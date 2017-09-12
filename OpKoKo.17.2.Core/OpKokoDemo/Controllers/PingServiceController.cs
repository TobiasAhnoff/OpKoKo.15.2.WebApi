using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpKokoDemo.Requests;
using OpKokoDemo.Responses;
using OpKokoDemo.Services;

namespace OpKokoDemo.Controllers
{
    [Route("myservice")]
    public class PingServiceController : Controller
    {
        private readonly IPingService _pingService;

        public PingServiceController(IPingService pingService)
        {
            _pingService = pingService;
        }
        // POST api/values
        [HttpPost("{id:int}/execute")]
        [Description("Ping uri with a retry limit")]
        public async Task<IActionResult> Post(int id, [FromBody] ExecuteServiceRequest request)
        {
            if (request == null)
                return BadRequest();

            var res = await _pingService.Execute(request);
            return new OkObjectResult(new ExecuteServiceResponse {Counter = res.Item1, ElapsedMilliseconds = res.Item2});
        }
    }
}
