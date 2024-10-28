﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Student
    {
        [Key]
        public string IdCard { get; set; }


        public string FirstName { get; set; }

        public string LastName { get; set; }

        //this is the foreign key so this contains a single value
        public string GroupFK { get; set; }

        //this is a navigational property
        //the navigational property will allow me to explore and navigate the
        //group properties right through an eventual student instance
        //adv: i can get data related to the Group pertaining to this student without having to write additional sql/linq statements
        [ForeignKey("GroupFK")]
        public virtual Group Group { get; set; }

        public string? ImagePath { get; set; }


    }
}
