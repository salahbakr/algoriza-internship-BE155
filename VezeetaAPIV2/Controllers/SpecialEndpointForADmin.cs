using Core.Repositories;
using Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VezeetaAPIV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialEndpointForADmin : ControllerBase
    {
        private readonly IAuthService authService;
        public SpecialEndpointForADmin(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpGet("DefaultAdminAccountForTesting")]
        public async Task<IActionResult> DefaultAdminAccountAssync()
        {
            return Ok(await authService.CreateAndLoginAsAdmin());
        }
    }
}
