using Core.Dtos.AuthenticationDtos;
using Core;
using Core.Dtos.DoctorDto;
using Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace VezeetaAPIV2.Controllers
{
    [Route("/doctor")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly IAuthService _authService;

        public DoctorController(IDoctorService doctorService, IAuthService authService)
        {
            _doctorService = doctorService;
            _authService = authService;
        }

        [HttpPost("AddAppointment")]
        public async Task<IActionResult> RegisterAppointmentAsync([FromForm] AppointmentDto appointment)
        {
            var userId = User.FindFirst("uid")?.Value;

            var result = await _doctorService.AddApointmentAsync(appointment, userId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromForm] LoginDto loginModel)
        {
            var result = await _authService.LoginAsync(loginModel);

            if (!result.Success)
                return BadRequest(result);

            if (result.Data.Roles.Contains("Doctor"))
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(new ResponseModel<AuthDto> { Message = "You are not a doctor" });
            }
        }
    }
}
