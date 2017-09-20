using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpKokoDemo.Services;

namespace OpKokoDemo.Controllers
{
    [Route("token")]
    public class TokenController : Controller
    {
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public string GetToken()
        {
            return _tokenService.GetToken();
        }
    }
}
