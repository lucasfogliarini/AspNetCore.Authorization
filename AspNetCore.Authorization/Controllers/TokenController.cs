using AspNetCore.Authorization.JsonWebToken;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Authorization.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController(IAuthService authorizationService, ILogger<WeatherForecastController> logger) : ControllerBase
    {
        private readonly IAuthService authorizationService = authorizationService;
        private readonly ILogger<WeatherForecastController> _logger = logger;

        [HttpPost]
        public async Task<IActionResult> Token(AuthenticationInput authenticationInput)
        {
            var jwt = authorizationService.CreateJwt(authenticationInput);
            return Ok(jwt);
        }
    }
}
