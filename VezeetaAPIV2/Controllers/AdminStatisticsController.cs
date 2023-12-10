using Core;
using Core.Repositories;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;
using System.Data;

namespace VezeetaAPIV2.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("admin/statistics")]
    [ApiController]
    public class AdminStatisticsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public AdminStatisticsController(ApplicationDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("Top5Specializations")]
        public async Task<IActionResult> GetTopSpecializations()
        {
            var specialization = _context.Specializations.OrderByDescending(x => x.Requests).Take(5).ToList();

            return Ok(specialization);
        }

        [HttpGet("NumberOfDoctors")]
        public async Task<IActionResult> GetNumberOfDoctors()
        {
            var count = await _unitOfWork.AuthRepository.GetUsersInRoleCount("Doctor");
            return Ok(new ResponseModel<object> { Message = "Retrieved Number of Doctors", Success = true, Data = count});
        }

        [HttpGet("NumberOfPatients")]
        public async Task<IActionResult> GetNumberOfPatients()
        {
            var count = await _unitOfWork.AuthRepository.GetUsersInRoleCount("Patient");
            return Ok(new ResponseModel<object> { Message = "Retrieved Number of Patients", Success = true, Data = count });
        }

        [HttpGet("NumberOfRequests")]
        public async Task<IActionResult> GetNumberOfRequests()
        {
            var numberOfRequests = await _context.Requests.CountAsync();
            return Ok(new ResponseModel<object> { Message = "Retrieved Number of Requests", Success = true, Data = numberOfRequests });
        }


    }
}
