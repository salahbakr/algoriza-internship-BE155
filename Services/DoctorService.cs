using AutoMapper;
using Core;
using Core.Dtos.DoctorDto;
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

        public IMapper Mapper { get; }

        public async Task<ResponseModel<AppointmentDto>> AddApointmentAsync(AppointmentDto appointmentDto, string doctorId)
        {
            var doctor = await _unitOfWork.AuthRepository.GetUserByIdAsync(doctorId);

            var appointment = _mapper.Map<Appointment>(appointmentDto);

            appointment.Doctor = doctor;

            try
            {
                await _unitOfWork.Appointments.CreateAsync(appointment);
            }
            catch (DbUpdateException)
            {
                return new ResponseModel<AppointmentDto> { Message = "Something went wrong while adding appointment" };
            }

            _unitOfWork.Complete();

            return new ResponseModel<AppointmentDto> { Success = true, Message = "Successfully added new appointment", Data = _mapper.Map<AppointmentDto>(appointment) };
        }
    }
}
