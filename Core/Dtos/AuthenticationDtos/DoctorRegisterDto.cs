using Core.ValidationAttributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Dtos.AuthenticationDtos
{
    public class DoctorRegisterDto
    {
        [Required]
        [MaxLength(50, ErrorMessage = "First name must not exceed 50 characters.")]
        [MinLength(4, ErrorMessage = "First name must not be less than 4 characters.")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Last name must not exceed 50 characters.")]
        [MinLength(4, ErrorMessage = "Last name must not be less than 4 characters.")]
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public int SpecializeId { get; set; }

        public IFormFile? ImageFile { get; set; }

        [Required]
        [DateOfBirthValidation]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Password { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Gender
    {
        Male,
        Female
    }
}
