﻿using Core.Dtos.AdminDtos;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IAdminService
    {
        Task<ResponseModel<IEnumerable<SpecializationDto>>> GetAllSpecializations(string search = "", int page = 1, int pageSize = 5);
        Task<ResponseModel<IEnumerable<DoctorDto>>> GetAllDoctorsAsync(string role, string search, int page = 1, int pageSize = 5);
        Task<ResponseModel<DoctorDto>> GetDoctorByIdAsync(string id);
        Task<ResponseModel<IEnumerable<PatientDto>>> GetAllPatientsAsync(string role, string search, int page = 1, int pageSize = 5);
        Task<ResponseModel<PatientDto>> GetPatientByIdAsync(string id);
        Task<ResponseModel<Coupon>> AddCoupon(CouponDto couponDto);
        Task<ResponseModel<IEnumerable<Coupon>>> GetAllCoupons(string search = "", int page = 1, int PageSize = 5);
        Task<ResponseModel<Coupon>> DeactivateCoupon(int couponId);
        Task<ResponseModel<Coupon>> DeleteCoupon(int couponId);
    }
}