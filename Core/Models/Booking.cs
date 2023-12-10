using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [JsonIgnore]
        public virtual ApplicationUser Patient { get; set; }

        [ForeignKey("TimeForeignKey")]
        public int TimeId { get; set; }
        public virtual DayTime Time { get; set; }

        [ForeignKey("RequestForeignKey")]
        public int RequestId { get; set; }
        public virtual Request Request { get; set; }
    }
}