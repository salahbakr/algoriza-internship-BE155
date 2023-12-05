﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ResponseModel<T> where T : class
    {
        public Metadata MetaData { get; set; }
        public bool Success { get; set; }

        public string? Message { get; set; }

        public T? Data { get; set; }
    }
}
