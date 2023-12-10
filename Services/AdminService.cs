using AutoMapper;
using Core;
using Core.Dtos.AdminDtos;
using Core.Models;
using Core.Repositories;
using Core.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public AdminService(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<ResponseModel<IEnumerable<SpecializationDto>>> GetAllSpecializations(string search = "", int page = 1, int pageSize = 5)
        {
            IEnumerable<Specialization> specializations = new List<Specialization>();
            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    specializations = await _unitOfWork.Specializations.GetAllPaginatedFilteredAsync(s => s.Name.Contains(search), page, pageSize);
                }
                else
                {
                    specializations = await _unitOfWork.Specializations.GetAllPaginatedFilteredAsync(null, page, pageSize);
                }
            }
            catch (DbUpdateException)
            {
                return new ResponseModel<IEnumerable<SpecializationDto>> { Message = "Something went wrong" };
            }

            var specializationsDto = _mapper.Map<IEnumerable<SpecializationDto>>(specializations);

            Metadata meta = new Metadata
            {
                Page = 1,
                PageSize = pageSize,
                Next = page + 1,
                Previous = page - 1
            };

            return new ResponseModel<IEnumerable<SpecializationDto>> { MetaData = meta, Success = true, Message = "Retrieved specializations", Data = specializationsDto };
        }

        public async Task<ResponseModel<IEnumerable<DoctorDto>>> GetAllDoctorsAsync(string role, string search, int page = 1, int pageSize = 5)
        {
            var users = await _unitOfWork.AuthRepository.GetAllUsersInRole("Doctor", search, page, pageSize);

            var doctorsDto = _mapper.Map<IEnumerable<DoctorDto>>(users);

            foreach (var doctor in doctorsDto)
            {
                doctor.Image = _imageService.GenerateUrl(doctor.Image);
            }

            Metadata meta = new Metadata
            {
                Page = 1,
                PageSize = pageSize,
                Next = page + 1,
                Previous = page - 1
            };

            return new ResponseModel<IEnumerable<DoctorDto>> { Message = "Retrieved doctors", Success = true, Data = doctorsDto, MetaData = meta };
        }

        public async Task<ResponseModel<IEnumerable<PatientDto>>> GetAllPatientsAsync(string role, string search, int page = 1, int pageSize = 5)
        {
            var users = await _unitOfWork.AuthRepository.GetAllUsersInRole("Patient", search, page, pageSize);

            var patientsDto = _mapper.Map<IEnumerable<PatientDto>>(users);

            foreach (var patient in patientsDto)
            {
                patient.Image = _imageService.GenerateUrl(patient.Image);
            }

            Metadata meta = new Metadata
            {
                Page = 1,
                PageSize = pageSize,
                Next = page + 1,
                Previous = page - 1
            };

            return new ResponseModel<IEnumerable<PatientDto>> { Message = "Retrieved patients", Success = true, Data = patientsDto, MetaData = meta };
        }

        public async Task<ResponseModel<DoctorDto>> GetDoctorByIdAsync(string id)
        {
            var doctor = await _unitOfWork.AuthRepository.GetUserByIdAsync(id);

            if (doctor is null)
            {
                return new ResponseModel<DoctorDto> { Message = "No doctor match that id: " };
            }

            var roles = await _unitOfWork.AuthRepository.GetRolesAsync(doctor);

            if (!roles.Contains("Doctor"))
            {
                return new ResponseModel<DoctorDto> { Message = "This is not a doctor!" };
            }

            var doctorDto = _mapper.Map<DoctorDto>(doctor);
            doctorDto.Image = _imageService.GenerateUrl(doctorDto.Image);
            
            return new ResponseModel<DoctorDto> { Message = "Retrieved doctor", Success = true, Data = doctorDto };
        }

        public async Task<ResponseModel<PatientDto>> GetPatientByIdAsync(string id)
        {
            var patient = await _unitOfWork.AuthRepository.GetUserByIdAsync(id);

            if (patient is null)
            {
                return new ResponseModel<PatientDto> { Message = "No patient match that id: " };
            }

            var roles = await _unitOfWork.AuthRepository.GetRolesAsync(patient);

            if (!roles.Contains("Patient"))
            {
                return new ResponseModel<PatientDto> { Message = "This is not a patient!" };
            }

            var patientDto = _mapper.Map<PatientDto>(patient);
            patientDto.Image = _imageService.GenerateUrl(patientDto.Image);

            return new ResponseModel<PatientDto> { Message = "Retrieved doctor", Success = true, Data = patientDto };
        }

        public async Task<ResponseModel<Coupon>> AddCoupon(CouponDto couponDto)
        {
            var coupon = _mapper.Map<Coupon>(couponDto);

            await _unitOfWork.Coupons.CreateAsync(coupon);

            try
            {
                _unitOfWork.Complete();
            }
            catch (DbUpdateException)
            {
                return new ResponseModel<Coupon> { Message = "Something went wrong" };
            }

            return new ResponseModel<Coupon> { Message = "Successfully added coupon" };
        }

        public async Task<ResponseModel<IEnumerable<Coupon>>> GetAllCoupons(string search = "", int page = 1, int PageSize = 5)
        {
            var coupons = await _unitOfWork.Coupons.GetAllPaginatedFilteredAsync(c => c.Name.Contains(search), page, PageSize);

            var meta = new Metadata
            {
                Page = page,
                PageSize = PageSize,
                Next = page + 1,
                Previous = page - 1
            };

            return new ResponseModel<IEnumerable<Coupon>> { Message = "Successfully added coupon", Success = true, Data = coupons, MetaData = meta };
        }

        public async Task<ResponseModel<Coupon>> DeactivateCoupon(int couponId)
        {
            var coupon = await _unitOfWork.Coupons.GetByIdAsync(couponId);

            if (coupon is null)
            {
                return new ResponseModel<Coupon> { Message = " no coupon match that id" };
            }

            coupon.IsActive = false;

            _unitOfWork.Coupons.Update(coupon);

            try
            {
                _unitOfWork.Complete();
            }
            catch (DbUpdateException)
            {
                return new ResponseModel<Coupon> { Message = "Something went wrong." };
            }

            return new ResponseModel<Coupon> { Message = "Successfully updated coupon", Success = true, Data = coupon};
        }

        public async Task<ResponseModel<Coupon>> DeleteCoupon(int couponId)
        {
            var coupon = await _unitOfWork.Coupons.GetByIdAsync(couponId);

            if (coupon is null)
            {
                return new ResponseModel<Coupon> { Message = " no coupon match that id" };
            }

            _unitOfWork.Coupons.Delete(coupon);

            try
            {
                _unitOfWork.Complete();
            }
            catch (DbUpdateException)
            {
                return new ResponseModel<Coupon> { Message = "Something went wrong." };
            }

            return new ResponseModel<Coupon> { Message = "Successfully deleted coupon", Success = true, Data = coupon};
        }
    }
}