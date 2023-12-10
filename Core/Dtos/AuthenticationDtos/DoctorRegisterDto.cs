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
    public class DoctorRegisterDto : BaseRegisterDto
    {
        [Required]
        public int SpecializeId { get; set; }
    }
}
