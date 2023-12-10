using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos.AdminDtos
{
    public class CouponDto
    {
        public string Name { get; set; }
        public int NumberOfRequests { get; set; }
        public bool IsActive { get; set; }
        public DiscoundType DiscoundType { get; set; }
        public int Discound { get; set; }
    }
}
