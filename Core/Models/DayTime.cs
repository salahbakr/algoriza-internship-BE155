using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Models
{
    public class DayTime
    {
        public int Id { get; set; }
        public TimeOnly Time { get; set; }

        [JsonIgnore]
        public virtual Appointment Appointment { get; set; }

        public virtual Booking? Booking { get; set; }
    }
}