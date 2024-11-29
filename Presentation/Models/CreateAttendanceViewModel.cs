using Domain.Models;

namespace Presentation.Models
{
    public class CreateAttendanceViewModel
    {

        public CreateAttendanceViewModel() {
            Attendances = new List<Attendance>(); //an empty
        }

        public List<Student> Students { get; set; } //1, 2, 3, 4, 5, 6, 7,. ...
        public List<Attendance> Attendances { get; set; } //{1, t, 1},{},{}
      
        public string SubjectCode { get; set; }

        public string SubjectName { get; set; }

        public string GroupCode { get; set; }
        

    }
}
