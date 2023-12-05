using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos.DoctorDto
{
    public class AppointmentDto
    {
        public int Price { get; set; }

        public Weekdays Weekdays { get; set; }

        public virtual ICollection<DayTime> Time { get; set; }
    }
}
