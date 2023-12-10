using Core.Dtos.DoctorDto;
using Core.Dtos.GeneralDto;
using Core.Dtos.PatientDto;
using Core.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IDoctorService
    {
        Task<ResponseModel<Appointment>> AddApointmentAsync(AppointmentDto appointment, string doctorId);
        Task<ResponseModel<IEnumerable<AppointmentDto2>>> GetAllApointmentsAsync(string doctorId, int page = 1, int pageSize = 5);
        Task<ResponseModel<IEnumerable<GetAllDoctorBooking>>> GetAllPendingBookingsAsync(string doctorId, int page = 1, int pageSize = 5);
        Task<ResponseModel<Appointment>> UpdateAppointmentAsync(int appointmentId, AppointmentDto appointmentDto, string doctorId);
        Task<ResponseModel<string>> ConfirmCheckUps(string doctorId, int bookingId);
        Task<ResponseModel<Appointment>> DeleteAppointmentAsync(int appointmentId, string doctorId);
    }
}
