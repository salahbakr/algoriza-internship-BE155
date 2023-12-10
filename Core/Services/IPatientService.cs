using Core.Dtos.PatientDto;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IPatientService
    {
        Task<ResponseModel<IEnumerable<GetAllDoctorsDto>>> GetAllAppointmentsAsync(string search, int page = 1, int pageSize = 5);
        Task<ResponseModel<BookingDto>> BookingAsync(string patientId, int timeId);
        Task<ResponseModel<IEnumerable<BookingDto>>> GetAllBooking(string patientId, int page = 1, int pageSize = 5);
        Task<ResponseModel<Booking>> CancelBooking(string patientId, int bookingId);
    }
}
