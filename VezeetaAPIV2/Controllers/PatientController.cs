using Core.Dtos.AuthenticationDtos;
using Core;
using Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Core.Models;

namespace VezeetaAPIV2.Controllers
{
    [Route("/patient")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IPatientService _petientService;

        public PatientController(IAuthService authService, IPatientService petientService)
        {
            _authService = authService;
            _petientService = petientService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromForm] PatientRegisterDto registerModel)
        {
            var result = await _authService.RegisterAsync(registerModel, "Patient");

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginModel)
        {
            var result = await _authService.LoginAsync(loginModel);

            if (!result.Success)
                return BadRequest(result);

            if (result.Data.Roles.Contains("Patient"))
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(new ResponseModel<AuthDto> { Message = "You are not a patient" });
            }
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("GetAllDoctorsAppointments")]
        public async Task<IActionResult> GetAllDoctorsAppointments(string search = "", int page = 1, int pageSize = 5)
        {
            var result = await _petientService.GetAllAppointmentsAsync(search, page, pageSize);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }  
        
        [Authorize(Roles = "Patient")]
        [HttpPost("Booking/id={timeId}")]
        public async Task<IActionResult> Booking(int timeId, int couponId = 0)
        {
            var userId = User.FindFirst("uid")?.Value;

            var result = await _petientService.BookingAsync(userId, timeId, couponId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        
        [Authorize(Roles = "Patient")]
        [HttpGet("Booking/GetAllPendingBookings")]
        public async Task<IActionResult> GetAllBookingsAsync(int page = 1, int pageSize = 5)
        {
            var userId = User.FindFirst("uid")?.Value;

            var result = await _petientService.GetAllBooking(userId, page, pageSize);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        
        [Authorize(Roles = "Patient")]
        [HttpDelete("Booking/CancelBooking/id={bookingId}")]
        public async Task<IActionResult> GetAllBookingsAsync(int bookingId)
        {
            var userId = User.FindFirst("uid")?.Value;

            var result = await _petientService.CancelBooking(userId, bookingId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        
        [Authorize(Roles = "Patient")]
        [HttpGet("Booking/AvailableCoupons")]
        public async Task<IActionResult> GetAvailableCoupons()
        {
            var userId = User.FindFirst("uid")?.Value;

            var result = await _petientService.AvailableCoupons(userId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
