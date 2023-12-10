using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Request
    {
        public int Id { get; set; }

        public Status Status { get; set; }

        public bool IsUsedForCoupon { get; set; }

        public virtual Booking Booking { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status
    {
        Pending,
        Completed,
        Cancelled
    }
}