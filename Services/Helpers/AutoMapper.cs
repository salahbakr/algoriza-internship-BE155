using AutoMapper;
using Core.Dtos.AdminDtos;
using Core.Dtos.AuthenticationDtos;
using Core.Dtos.DoctorDto;
using Core.Dtos.GeneralDto;
using Core.Dtos.PatientDto;
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
            
            CreateMap<PatientRegisterDto, ApplicationUser>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            CreateMap<ApplicationUser, DoctorDto>()
                .ForMember(dest => dest.Specialize, src => src.MapFrom(src => src.Specialize.Name)); 
            
            CreateMap<ApplicationUser, PatientDto>();

            CreateMap<EditDto, ApplicationUser>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            CreateMap<Specialization, SpecializationDto>();

            CreateMap<AppointmentDto, Appointment>()
                .ForMember(dest => dest.Time, opt => opt.Ignore())
                .ReverseMap();  

            CreateMap<Appointment, AppointmentDto2>()
                .ReverseMap();  

            CreateMap<Request, RequestDto>()
                .ReverseMap();  
            
            CreateMap<DayTime, DayTimeDto>()
                .ReverseMap();   
            
            CreateMap<Coupon, CouponDto>()
                .ReverseMap();   

            CreateMap<Booking, GetAllDoctorBooking>()
                .ForMember(dest => dest.PatientName, src => src.MapFrom(src => src.Patient.FirstName + " " + src.Patient.LastName))
                .ForMember(dest => dest.Price, src => src.MapFrom(src => src.Time.Appointment.Price))
                .ForMember(dest => dest.FinalPrice, src => src.MapFrom(src => src.FinalPrice))
                .ForMember(dest => dest.Gender, src => src.MapFrom(src => src.Patient.Gender))
                .ForMember(dest => dest.DateOfBirth, src => src.MapFrom(src => src.Patient.DateOfBirth))
                .ForMember(dest => dest.Gender, src => src.MapFrom(src => src.Patient.Gender))
                .ForMember(dest => dest.Time, src => src.MapFrom(src => new DayTimeDto { Id = src.TimeId, Time = src.Time.Time}))
                .ForMember(dest => dest.Request, src => src.MapFrom(src => new RequestDto { Id = src.Request.Id, Status = src.Request.Status}))
                .ReverseMap();

            CreateMap<Booking, BookingDto2>()
                .ForMember(dest => dest.PatientId, src => src.MapFrom(src => src.Patient.Id))
                .ForMember(dest => dest.PatientName, src => src.MapFrom(src => src.Patient.FirstName + " " + src.Patient.LastName))
                .ForMember(dest => dest.FinalPrice, src => src.MapFrom(src => src.FinalPrice))
                .ReverseMap();


            CreateMap<Booking, BookingDto>()
                .ForMember(dest => dest.TimeId, src => src.MapFrom(src => src.Time.Id))
                .ForMember(dest => dest.Time, src => src.MapFrom(src => src.Time.Time))
                .ForMember(dest => dest.DoctorName, src => src.MapFrom(src => src.Time.Appointment.Doctor.FirstName + " " + src.Time.Appointment.Doctor.LastName))
                .ForMember(dest => dest.RequestStatus, src => src.MapFrom(src => src.Request.Status))
                .ForMember(dest => dest.Weekday, src => src.MapFrom(src => src.Time.Appointment.Weekdays))
                .ForMember(dest => dest.Price, src => src.MapFrom(src => src.Time.Appointment.Price))
                .ForMember(dest => dest.FinalPrice, src => src.MapFrom(src => src.FinalPrice))
                .ReverseMap();

            CreateMap<ApplicationUser, GetAllDoctorsDto>();
        }
    }
}