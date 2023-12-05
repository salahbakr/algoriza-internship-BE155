﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Specialization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Requests { get; set; }

        public virtual ICollection<ApplicationUser>? Doctors { get; set; }
    }
}