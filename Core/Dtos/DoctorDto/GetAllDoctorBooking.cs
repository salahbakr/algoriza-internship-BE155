using Core.Dtos.AdminDtos;
using Core.Dtos.GeneralDto;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos.DoctorDto
{
    public class GetAllDoctorBooking
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public AdminDtos.Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        public virtual DayTimeDto Time { get; set; }

        public virtual RequestDto Request { get; set; }
    }
}
