using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Request
    {
        public int Id { get; set; }

        public Status Status { get; set; }

        public string PatientId { get; set; }
        public virtual ApplicationUser Doctor { get; set; }
    }

    public enum Status
    {
        Pending,
        Completed,
        Cancelled
    }
}