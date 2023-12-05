using Core;
using Core.Dtos.AuthenticationDtos;
using Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VezeetaAPIV2.Controllers
{
    [Route("admin/doctor")]
    [ApiController]
    public class AdminDoctorController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAdminService _adminService;

        public AdminDoctorController(IAuthService authService, IAdminService adminService)
        {
            _authService = authService;
            _adminService = adminService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> RegisterAsync([FromForm] DoctorRegisterDto registerModel)
        {
            var result = await _authService.RegisterAsync(registerModel, "Doctor");

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }    
        
        [HttpPut("edit")]
        public async Task<IActionResult> RegisterAsync([FromForm] EditDto updateModel)
        {
            var result = await _authService.UpdateAsync(updateModel);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("GetAllSpecializations")]
        public async Task<IActionResult> GetAllSpecializations(string search = null, int page = 1, int pageSize = 5)
        {
            var result = await _adminService.GetAllSpecializations(search, page, pageSize);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }     
        
        [HttpGet("GetAllDoctors")]
        public async Task<IActionResult> GetAllDoctors(string search = "", int page = 1, int pageSize = 5)
        {
            var result = await _adminService.GetAllDoctorsAsync("Doctor", search, page, pageSize);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("GetDoctor/id={doctorId}")]
        public async Task<IActionResult> GetDoctorById(string doctorId)
        {
            var result = await _adminService.GetDoctorByIdAsync(doctorId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("DeleteDoctor/id={doctorId}")]
        public async Task<IActionResult> DeleteDoctorById(string doctorId)
        {
            var result = await _authService.DeleteAsync(doctorId, "Doctor");

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
