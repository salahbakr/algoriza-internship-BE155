using Core.Dtos.DoctorDto;
using Core.Dtos.GeneralDto;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Dtos.PatientDto
{
    public class GetAllDoctorsDto
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public Gender Gender { get; set; }

        public string Specialize { get; set; }

        public string? Image { get; set; }

        public ICollection<AppointmentDto2>? Appointments { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Gender
    {
        Male,
        Female
    }
}
