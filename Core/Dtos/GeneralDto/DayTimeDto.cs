using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos.GeneralDto
{
    public class DayTimeDto
    {
        public int Id { get; set; }
        public TimeOnly Time { get; set; }

        public BookingDto2? Booking { get; set; }
    }
}
