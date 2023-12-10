using AutoMapper;
using Core;
using Core.Dtos.DoctorDto;
using Core.Dtos.GeneralDto;
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
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DoctorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseModel<Appointment>> AddApointmentAsync(AppointmentDto appointmentDto, string doctorId)
        {
            var doctor = await _unitOfWork.AuthRepository.GetUserByIdAsync(doctorId);

            var appointment = _mapper.Map<Appointment>(appointmentDto);

            appointment.Doctor = doctor;
            appointment.Time = appointmentDto.TimeOnly.Select(t => new DayTime { Time = t}).ToList();

            var existingAppointments = await _unitOfWork.Appointments.GetAllPaginatedFilteredAsync(
                a => a.Doctor.Id == doctorId && a.Weekdays == appointmentDto.Weekdays, 1, 7);

            if (existingAppointments.Count() >= 1)
            {
                return new ResponseModel<Appointment> { Message = $"You do already have an appointment for {appointmentDto.Weekdays}" };
            }

            try
            {
                await _unitOfWork.Appointments.CreateAsync(appointment);
            }
            catch (DbUpdateException)
            {
                return new ResponseModel<Appointment> { Message = "Something went wrong while adding appointment" };
            }

            _unitOfWork.Complete();

            return new ResponseModel<Appointment> { Success = true, Message = "Successfully added new appointment", Data = appointment };
        }

        public async Task<ResponseModel<IEnumerable<AppointmentDto2>>> GetAllApointmentsAsync(string doctorId, int page = 1, int pageSize = 5)
        {
            var appointments = await _unitOfWork.Appointments.GetAllPaginatedFilteredAsync(a => a.Doctor.Id == doctorId, page, pageSize);

            Metadata meta = new Metadata
            {
                Page = page,
                PageSize = pageSize,
                Next = page + 1,
                Previous = page - 1
            };

            return new ResponseModel<IEnumerable<AppointmentDto2>> { Success = true, Message = "Retrieved appointments", Data = _mapper.Map<IEnumerable<AppointmentDto2>>(appointments), MetaData = meta };
        }

        public async Task<ResponseModel<IEnumerable<GetAllDoctorBooking>>> GetAllPendingBookingsAsync(string doctorId, int page = 1, int pageSize = 5)
        {
            var bookings = await _unitOfWork.Bookings.GetAllPaginatedFilteredAsync(b => b.Time.Appointment.Doctor.Id == doctorId, page, pageSize);

            Metadata meta = new Metadata
            {
                Page = page,
                PageSize = pageSize,
                Next = page + 1,
                Previous = page - 1
            };

            return new ResponseModel<IEnumerable<GetAllDoctorBooking>> { Success = true, Message = "Retrieved appointments", Data = _mapper.Map<IEnumerable<GetAllDoctorBooking>>(bookings), MetaData = meta };
        }

        public async Task<ResponseModel<Appointment>> UpdateAppointmentAsync(int appointmentId, AppointmentDto appointmentDto, string doctorId)
        {
            var doctor = await _unitOfWork.AuthRepository.GetUserByIdAsync(doctorId);

            var appointment = await _unitOfWork.Appointments.GetByIdAsync(appointmentId);

            foreach (var time in appointment.Time)
            {
                if (time.Booking is not null)
                {
                    return new ResponseModel<Appointment> { Message = "You can not update this appointment because a patient already booked it." };
                }
            }
            
            _mapper.Map(appointmentDto, appointment);

            appointment.Doctor = doctor;
            appointment.Time = appointmentDto.TimeOnly.Select(t => new DayTime { Time = t }).ToList();

            try
            {
                _unitOfWork.Appointments.Update(appointment);
            }
            catch (DbUpdateException)
            {
                return new ResponseModel<Appointment> { Message = "Something went wrong while updating appointment" };
            }

            _unitOfWork.Complete();

            return new ResponseModel<Appointment> { Success = true, Message = "Successfully updated appointment", Data = appointment };
        }

        public async Task<ResponseModel<string>> ConfirmCheckUps(string doctorId, int bookingId)
        {
            var doctor = await _unitOfWork.AuthRepository.GetUserByIdAsync(doctorId);

            var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId);

            if (booking is null)
            {
                return new ResponseModel<string> { Message = "No booking match that id" };
            }

            if (!doctor.Appointments.Any(a => a.Time.Any(t => t.Booking.Id == booking.Id)))
            {
                return new ResponseModel<string> { Message = "This booking does not belong to you." };
            }

            var request = booking.Request;

            _unitOfWork.Bookings.Delete(booking);

            try
            {
                request.Status = Status.Completed;
                _unitOfWork.Complete();
            }
            catch (DbUpdateException)
            {
                return new ResponseModel<string> { Message = "Something went wrong!." };
            }

            return new ResponseModel<string> { Message = "Confirmed booking and completed request.", Success = true, Data = "" };
        }

        public async Task<ResponseModel<Appointment>> DeleteAppointmentAsync(int appointmentId, string doctorId)
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(appointmentId);

            if (appointment is null)
            {
                return new ResponseModel<Appointment> { Message = "No appointment match that id" };
            }

            if (appointment.Doctor.Id != doctorId)
            {
                return new ResponseModel<Appointment> { Message = "You can not delete this appointment because it belongs to another doctor." };
            }

            foreach (var time in appointment.Time)
            {
                if (time.Booking is not null)
                {
                    return new ResponseModel<Appointment> { Message = "You can not delete this appointment because a patient already booked it." };
                }
            }

            try
            {
                _unitOfWork.Appointments.Delete(appointment);
            }
            catch (DbUpdateException)
            {
                return new ResponseModel<Appointment> { Message = "Something went wrong while deleting appointment" };
            }

            _unitOfWork.Complete();

            return new ResponseModel<Appointment> { Success = true, Message = "Successfully deleted appointment", Data = appointment };
        }
    }
}
