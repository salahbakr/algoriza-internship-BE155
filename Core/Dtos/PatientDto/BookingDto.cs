using Core.Dtos.DoctorDto;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos.PatientDto
{
    public class BookingDto
    {
        public int Id { get; set; }
        public int TimeId { get; set; }
        public TimeOnly Time { get; set; }
        public Weekdays Weekday { get; set; }
        public Status RequestStatus { get; set; }
        public string DoctorName { get; set; }
    }
}
