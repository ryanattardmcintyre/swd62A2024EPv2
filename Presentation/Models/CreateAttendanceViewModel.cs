using Domain.Models;

namespace Presentation.Models
{
    public class CreateAttendanceViewModel
    {

        public CreateAttendanceViewModel() {
            Presence = new List<bool>(); //Count ==0 
        }

        public List<Student> Students { get; set; } //1, 2, 3, 4, 5, 6, 7,. ...
        public List<bool> Presence { get; set; } //T, T, F

        public string SubjectCode { get; set; }

        public string SubjectName { get; set; }

        public string GroupCode { get; set; }
        

    }
}
