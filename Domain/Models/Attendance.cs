using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Attendance
    {
        [Key]
        public int Id { get; set; }
        
        //Foreign key
        public string StudentFK { get; set; } //idcard

        //navigational property
        [ForeignKey("StudentFK")]
        public virtual Student Student { get; set; }

        public bool Present { get; set; } //presence

        public DateTime Timestamp { get; set; }

        public string SubjectFK { get; set; } //subject code
        
        [ForeignKey("SubjectFK")]
        public virtual Subject Subject { get; set; }



    }
}
