using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos.GeneralDto
{
    public class BookingDto2
    {
        public int Id { get; set; }

        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public int FinalPrice { get; set; }

        public RequestDto Request { get; set; }
    }
}
