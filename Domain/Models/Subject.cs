﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Subject
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }


    }
}
