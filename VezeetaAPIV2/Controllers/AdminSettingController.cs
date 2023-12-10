using Core.Dtos.AdminDtos;
using Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VezeetaAPIV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminSettingController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminSettingController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("AddCoupon")]
        public async Task<IActionResult> AddCouponAsync([FromForm] CouponDto dto)
        {
            var result = await _adminService.AddCoupon(dto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }   
        
        [HttpGet("GetAllCoupons")]
        public async Task<IActionResult> GetAllCouponsAsync(string search = "", int page = 1, int pageSize = 5)
        {
            var result = await _adminService.GetAllCoupons(search, page, pageSize);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        
        [HttpPut("DeactivateCoupon/id={couponId}")]
        public async Task<IActionResult> DeactivateCoupon(int couponId)
        {
            var result = await _adminService.DeactivateCoupon(couponId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        
        [HttpPut("DeleteCoupon/id={couponId}")]
        public async Task<IActionResult> DeleteCoupon(int couponId)
        {
            var result = await _adminService.DeactivateCoupon(couponId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
