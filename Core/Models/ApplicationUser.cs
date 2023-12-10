using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(50, ErrorMessage = "First name must not exceed 50 characters.")]
        [MinLength(4, ErrorMessage = "First name must not be less than 4 characters.")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Last name must not exceed 50 characters.")]
        [MinLength(4, ErrorMessage = "Last name must not be less than 4 characters.")]
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string? Image { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        public virtual Specialization? Specialize { get; set; }

        [JsonIgnore]
        public virtual ICollection<Request>? Requests { get; set; }

        [JsonIgnore]
        public virtual ICollection<Appointment>? Appointments { get; set; }

        [JsonIgnore]
        public virtual ICollection<Booking>? Bookings { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}
