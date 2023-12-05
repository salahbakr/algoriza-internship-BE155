using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string DoctorId { get; set; }

        public virtual ApplicationUser Patient { get; set; }

        [ForeignKey("AppointmentForeignKey")]
        public int AppointmentId { get; set; }
        public virtual Appointment Appointment { get; set; }

        public virtual Request Request { get; set; }
    }
}