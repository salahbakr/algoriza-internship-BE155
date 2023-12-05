using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int Price { get; set; }

        public Weekdays Weekdays { get; set; }

        public virtual ICollection<DayTime> Time{ get; set; }

        public virtual ApplicationUser Doctor { get; set; }

        [ForeignKey("BookingForeignKey")]
        public int BookingId { get; set; }
        public virtual Booking? Booking { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Weekdays
    {
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
    }
}