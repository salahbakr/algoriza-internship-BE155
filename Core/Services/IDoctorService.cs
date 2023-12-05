using Core.Dtos.DoctorDto;
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
        Task<ResponseModel<AppointmentDto>> AddApointmentAsync(AppointmentDto appointment, string doctorId);
    }
}
