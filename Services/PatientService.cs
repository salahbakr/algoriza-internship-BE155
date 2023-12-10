using AutoMapper;
using Core;
using Core.Dtos.PatientDto;
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

        public async Task<ResponseModel<BookingDto>> BookingAsync(string patientId, int timeId)
        {
            var patient = await _unitOfWork.AuthRepository.GetUserByIdAsync(patientId);

            var time = await _unitOfWork.Time.GetByIdAsync(timeId);

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

            await _unitOfWork.Bookings.CreateAsync(booking);

            patient.Requests.Add(request);

            time.Appointment.Doctor.Specialize.Requests = time.Appointment.Doctor.Specialize.Requests + 1;

            try
            {
                _unitOfWork.Complete();
            }
            catch (DbUpdateException)
            {
                new ResponseModel<BookingDto> { Message = "Something went wrong" };
            }

            var bookingDto = new BookingDto
            {
                TimeId = timeId,
                Time = time.Time,
                RequestStatus = booking.Request.Status,
                DoctorName = time.Appointment.Doctor.FirstName + " " + time.Appointment.Doctor.LastName,
            };

            return new ResponseModel<BookingDto> { Message = "Successfully booked appointment", Success = true, Data = bookingDto };
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

            if (!patient.Bookings.Any(b => b.Id == booking.Id))
            {
                return new ResponseModel<Booking> { Message = "This booking does not belong to you!" };
            }

            var request = booking.Request;

            _unitOfWork.Bookings.Delete(booking);

            try
            {
                request.Status = Status.Cancelled;
                _unitOfWork.Complete();
            }
            catch (DbUpdateException)
            {
                return new ResponseModel<Booking> { Message = "Something went wrong." };
            }

            return new ResponseModel<Booking> { Success = true, Message = "Cancelled booking", Data = booking };
        }
    }
}
