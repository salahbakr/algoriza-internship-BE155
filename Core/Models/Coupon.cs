using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Coupon
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<ApplicationUser> Patients { get; set; }

        public int NumberOfRequests { get; set; }
        public bool IsActive { get; set; }

        public DiscoundType DiscoundType { get; set; }

        public int Discound { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DiscoundType
    {
        Percentage,
        Value
    }
}
