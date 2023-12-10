using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos.GeneralDto
{
    public class AppointmentDto2
    {
        public int Id { get; set; }
        public int Price { get; set; }

        public Weekdays Weekdays { get; set; }

        public ICollection<DayTimeDto> Time { get; set; }
    }
}
