using Core.Dtos.AuthenticationDtos;
using Core;
using Core.Dtos.DoctorDto;
using Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;

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

        [Authorize(Roles = "Doctor")]
        [HttpGet("GetAllAppointments")]
        public async Task<IActionResult> GetAllAppointmentsAsync(int page = 1, int pageSize = 5)
        {
            var userId = User.FindFirst("uid")?.Value;

            var result = await _doctorService.GetAllApointmentsAsync(userId, page, pageSize);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet("GetAllPendingBookings")]
        public async Task<IActionResult> GetAllPendingBookingsAsync(int page = 1, int pageSize = 5)
        {
            var userId = User.FindFirst("uid")?.Value;

            var result = await _doctorService.GetAllPendingBookingsAsync(userId, page, pageSize);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize(Roles = "Doctor")]
        [HttpPut("ConfirmBooking/id={bookingId}")]
        public async Task<IActionResult> ConfirmCheckupsAsync(int bookingId)
        {
            var userId = User.FindFirst("uid")?.Value;

            var result = await _doctorService.ConfirmCheckUps(userId, bookingId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost("AddAppointment")]
        public async Task<IActionResult> AddAppointmentAsync([FromForm] AppointmentDto appointment)
        {
            var userId = User.FindFirst("uid")?.Value;

            var result = await _doctorService.AddApointmentAsync(appointment, userId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }   
        
        [Authorize(Roles = "Doctor")]
        [HttpPut("UpdateAppointment/id={appointmentId}")]
        public async Task<IActionResult> UpdateAppointmentAsync([FromForm] AppointmentDto appointment, int appointmentId)
        {
            var userId = User.FindFirst("uid")?.Value;

            var result = await _doctorService.UpdateAppointmentAsync(appointmentId, appointment, userId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize(Roles = "Doctor")]
        [HttpDelete("DeleteAppointment/id={appointmentId}")]
        public async Task<IActionResult> DeleteAppointmentAsync(int appointmentId)
        {
            var userId = User.FindFirst("uid")?.Value;

            var result = await _doctorService.DeleteAppointmentAsync(appointmentId, userId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginModel)
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
