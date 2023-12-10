using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class UsedCoupons
    {
        public int Id { get; set; }
        public string PatientId { get; set; }
        public virtual Coupon Coupoun { get; set; }
    }
}