using AutoMapper;
using Core.Dtos.AdminDtos;
using Core.Dtos.AuthenticationDtos;
using Core.Dtos.DoctorDto;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<DoctorRegisterDto, ApplicationUser>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            CreateMap<ApplicationUser, DoctorDto>()
                .ForMember(dest => dest.Specialize, src => src.MapFrom(src => src.Specialize.Name)); 
            
            CreateMap<ApplicationUser, PatientDto>();

            CreateMap<EditDto, ApplicationUser>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            CreateMap<Specialization, SpecializationDto>();

            CreateMap<AppointmentDto, Appointment>().ReverseMap();
        }
    }
}