using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

namespace VezeetaAPIV2.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("/admin/patient")]
    [ApiController]
    public class AdminPatientController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminPatientController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("GetAllPatients")]
        public async Task<IActionResult> GetAllPatientsAsync(string search = "", int page = 1, int pageSize = 5)
        {
            var result = await _adminService.GetAllPatientsAsync("Patient", search, page, pageSize);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("GetPatient/id={patientId}")]
        public async Task<IActionResult> GetPatientById(string patientId)
        {
            var result = await _adminService.GetPatientByIdAsync(patientId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
