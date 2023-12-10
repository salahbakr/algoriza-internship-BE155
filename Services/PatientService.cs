using AutoMapper;
using Core;
using Core.Dtos.PatientDto;
using Core.Models;
using Core.Repositories;
using Core.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public PatientService(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;
        }

        public IImageService ImageService { get; }

        public async Task<ResponseModel<BookingDto>> BookingAsync(string patientId, int timeId, int couponId = 0)
        {
            var patient = await _unitOfWork.AuthRepository.GetUserByIdAsync(patientId);

            var time = await _unitOfWork.Time.GetByIdAsync(timeId);

            if (time is null)
            {
                return new ResponseModel<BookingDto> { Message = "No time match that id" };
            }

            if (time.Booking is not null)
            {
                return new ResponseModel<BookingDto> { Message = "This time has been already booked" };
            }

            var request = new Request
            {
                Status = Status.Pending,
            };

            Booking booking = new Booking
            {
                Patient = patient,
                Time = time,
                Request = request
            };

            request.Booking = booking;

            patient.Requests.Add(request);

            time.Appointment.Doctor.Specialize.Requests = time.Appointment.Doctor.Specialize.Requests + 1;

            var result = await CalculateFinalPriceForCoupon(couponId, patient, patientId, booking, time);

            return result;
        }

        public async Task<ResponseModel<IEnumerable<GetAllDoctorsDto>>> GetAllAppointmentsAsync(string search, int page = 1, int pageSize = 5)
        {
            var doctors = await _unitOfWork.AuthRepository.GetAllUsersInRole("Doctor", search, page, pageSize);

            foreach (var doctor in doctors)
            {
                doctor.Image = _imageService.GenerateUrl(doctor.Image);
            }

            var doctorsAppointments = _mapper.Map<IEnumerable<GetAllDoctorsDto>>(doctors);

            var meta = new Metadata
            {
                Page = page,
                PageSize = pageSize,
                Next = page + 1,
                Previous = page - 1
            };

            return new ResponseModel<IEnumerable<GetAllDoctorsDto>> { Message = "Retrieved doctors with their appointments", Success = true, Data = doctorsAppointments, MetaData = meta };
        }

        public async Task<ResponseModel<IEnumerable<BookingDto>>> GetAllBooking(string patientId, int page = 1, int pageSize = 5)
        {
            var bookings = await _unitOfWork.Bookings.GetAllPaginatedFilteredAsync(b => b.Patient.Id == patientId, page, pageSize);

            var meta = new Metadata
            {
                Page = page,
                PageSize = pageSize,
                Next = page + 1,
                Previous = page - 1
            };

            return new ResponseModel<IEnumerable<BookingDto>> 
            {
                Message = "Retrieved bookings",
                Success = true,
                Data = _mapper.Map<IEnumerable<BookingDto>>(bookings),
                MetaData = meta
            };
        }

        public async Task<ResponseModel<Booking>> CancelBooking(string patientId, int bookingId)
        {
            var patient = await _unitOfWork.AuthRepository.GetUserByIdAsync(patientId);

            var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId);

            if (booking is null)
            {
                return new ResponseModel<Booking> { Message = "No booking match that id" };
            }

            var time = await _unitOfWork.Time.GetByIdAsync(booking.TimeId);

            if (!patient.Bookings.Any(b => b.Id == booking.Id))
            {
                return new ResponseModel<Booking> { Message = "This booking does not belong to you!" };
            }

            var request = booking.Request;

            _unitOfWork.Bookings.Delete(booking);

            try
            {
                request.Status = Status.Cancelled;
                time.Booking = null;
                _unitOfWork.Complete();
            }
            catch (DbUpdateException)
            {
                return new ResponseModel<Booking> { Message = "Something went wrong." };
            }

            return new ResponseModel<Booking> { Success = true, Message = "Cancelled booking", Data = booking };
        }

        public async Task<ResponseModel<List<Coupon>>> AvailableCoupons(string patientId)
        {
            var coupons = await _unitOfWork.Coupons.GetAllAsync();

            List<Coupon> availableCoupons = new List<Coupon>();

            var patientUsedCoupons = await _unitOfWork.UsedCoupons.GetAllByPropertyAsync(u => u.PatientId == patientId);

            foreach (var coupon in coupons)
            {
                foreach (var patient in coupon.Patients)
                {
                    if (patient.Id == patientId)
                    {
                        foreach (var usedCoupon in patientUsedCoupons)
                        {
                            if (usedCoupon.Coupoun != coupon)
                            {
                                availableCoupons.Add(coupon);
                            }
                        }
                    }
                }
            }

            return new ResponseModel<List<Coupon>> { Message = "Available to use coupons", Success = true, Data = availableCoupons };
        }

        internal async Task<ResponseModel<BookingDto>> CalculateFinalPriceForCoupon(int couponId, ApplicationUser patient, string patientId, Booking booking, DayTime time)
        {
            
            if (couponId != 0)
            {
                var coupon = await _unitOfWork.Coupons.GetByIdAsync(couponId);

                if (coupon is null || !coupon.IsActive)
                {
                    return new ResponseModel<BookingDto> { Message = "Coupon is no longer active or the coupon id is invalid" };
                }

                if (coupon.Patients.Where(p => p.Id == patientId).Count() == 0)
                {
                    return new ResponseModel<BookingDto> { Message = "You did not get that coupon yet." };
                }

                var usedCoupons = await _unitOfWork.UsedCoupons.GetAllByPropertyAsync(u => u.PatientId == patientId);

                foreach (var usedCoupon in usedCoupons)
                {
                    if (usedCoupon.Coupoun == coupon)
                    {
                        return new ResponseModel<BookingDto> { Message = "You have already used that coupon one time!" };
                    }
                }

                if (coupon.DiscoundType == DiscoundType.Value)
                {
                    booking.FinalPrice = time.Appointment.Price - coupon.Discound;
                }
                else
                {
                    booking.FinalPrice = time.Appointment.Price * (coupon.Discound / 100);
                }

                foreach (var patientRequest in patient.Requests)
                {
                    patientRequest.IsUsedForCoupon = true;
                }

                await _unitOfWork.UsedCoupons.CreateAsync(new UsedCoupons
                {
                    PatientId = patientId,
                    Coupoun = coupon
                });
            }
            else
            {
                var coupons = await _unitOfWork.Coupons.GetAllAsync();

                foreach (var availableCoupons in coupons)
                {
                    if (availableCoupons.NumberOfRequests == patient.Requests.Where(r => r.Status == Status.Completed && !r.IsUsedForCoupon).Count())
                    {
                        availableCoupons.Patients.Add(patient);
                    }
                }
                booking.FinalPrice = time.Appointment.Price;
            }

            await _unitOfWork.Bookings.CreateAsync(booking);
            try
            {
                _unitOfWork.Complete();
            }
            catch (DbUpdateException)
            {
                return new ResponseModel<BookingDto> { Message = "Something went wrong whil calculating discound" };
            }

            var bookingDto = new BookingDto
            {
                Price = time.Appointment.Price,
                FinalPrice = booking.FinalPrice,
                Id = booking.Id,
                TimeId = time.Id,
                Time = time.Time,
                RequestStatus = booking.Request.Status,
                DoctorName = time.Appointment.Doctor.FirstName + " " + time.Appointment.Doctor.LastName,
            };

            return new ResponseModel<BookingDto> { Message = "Successfully booked appointment", Success = true, Data = bookingDto };
        }
    }
}
