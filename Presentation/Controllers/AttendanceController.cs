using DataAccess.DataContext;
using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class AttendanceController : Controller
    {
        AttendancesRepository _attendancesRepository;
        GroupsRepository  _groupsRepository;
        SubjectsRepository _subjectsRepository;
        StudentsRepository _studentsRepository;
        public AttendanceController(AttendancesRepository attendancesRepository
            , GroupsRepository groupsRepository
            , SubjectsRepository subjectsRepository
            , StudentsRepository studentsRepository
            , ILogsRepository logsRepository
            ) {
            
            _attendancesRepository= attendancesRepository;
            _groupsRepository= groupsRepository;
            _subjectsRepository= subjectsRepository;
            _studentsRepository= studentsRepository;

        }

      

   
        //a page which shows me which attendances i can take 
        public IActionResult Index()
        {
            //a history of attendances
            /*   var groupedAttendances = _attendancesRepository.GetAttendances()
                    .GroupBy(a => new { a.SubjectFK, a.Student.GroupFK, a.Timestamp.Date, Subject = a.Subject.Name,
                      })
                    .OrderByDescending(x=>x.Key.Date)
                    .Select(g => new AttendancesListViewModel()   //anonymous type
                    {
                        SubjectFK = g.Key.SubjectFK,
                        SubjectName = g.Key.Subject,
                        Date = g.Key.Date,
                        Group = g.Key.GroupFK
                    })
                    .ToList();
            */

            var subjects = _subjectsRepository.GetSubjects();
            var groups = _groupsRepository.GetGroups();

            //Time | Group | Subject
            List<SelectPastAttendancesViewModel> pastAttendances = _attendancesRepository.GetAttendances()
                 .GroupBy(x => new
                 {   Date = x.Timestamp, 
                     SubjectCode = x.Subject.Code, //we cannot group by an entire object called Subject
                     GroupCode = x.Student.GroupFK,
                     SubjectName = x.Subject.Name //but we can use the individual columns/properties
                 })
                  .Select(group => new SelectPastAttendancesViewModel()
                  {
                      SubjectCode = group.Key.SubjectCode,
                      Date = group.Key.Date,
                      GroupCode = group.Key.GroupCode,
                      SubjectName = group.Key.SubjectName
                  }).
                 ToList();

         /*   foreach (var attendance in pastAttendances)
            {
                attendance.SubjectName = 
                    _subjectsRepository.GetSubjects().SingleOrDefault(x => x.Code == attendance.SubjectCode).Name;
            }
         */

            SelectGroupSubjectViewModel viewModel = new SelectGroupSubjectViewModel();
            viewModel.Subjects = subjects.ToList();
            viewModel.Groups = groups.ToList();
            viewModel.PastAttendances = pastAttendances;

            //To Do: show past attendances on the Index page

            return View(viewModel);
        }

        [HttpGet] //it needs to show me which students are supposed to be in that attendance list
        public IActionResult Create(string groupCode, string subjectCode, string whichButton)
        {
            //to do:
            //a list of students pertaining to the group
            //group code
            //subject code/name

            if (whichButton=="0")
            { // -------------------------- create -----------------------
                var students = _studentsRepository.GetStudents() //Select * From Students
                                .Where(x => x.GroupFK == groupCode) //Select * From Students Where GroupFK = groupCode
                                .OrderBy(x => x.FirstName)  //Select * From Students Where GroupFK = groupCode order by FirstName
                                .ToList(); //here is where the execution i.e. opening a connection to db actually happens

                CreateAttendanceViewModel myModel = new CreateAttendanceViewModel();
                myModel.SubjectCode = subjectCode;
                myModel.Students = students;
                myModel.GroupCode = groupCode;

                Subject mySubject = _subjectsRepository.GetSubjects().SingleOrDefault(x => x.Code == subjectCode); //Select * From Subjects Where Code == subjectCode
                if (mySubject == null)
                    myModel.SubjectName = ""; //we throw an exception, we do exception handling or we redirect the user to an error page
                else
                    myModel.SubjectName = mySubject.Name;

                
                ViewBag.update = false; //on the fly = it will be created when the application runs //another approach how to pass data to views

                return View(myModel);
            }
            else
            { // ---------------- update -------------------
                string[] myValues = whichButton.Split(new char[] { '|' });
                DateTime date = Convert.ToDateTime(myValues[0]);
                string selectedSubjectCode = myValues[1];
                string selectedGroupCode = myValues[2];

                CreateAttendanceViewModel myModel = new CreateAttendanceViewModel();
                myModel.SubjectCode = selectedSubjectCode;
                myModel.GroupCode = selectedGroupCode;
                myModel.Students = _studentsRepository.GetStudents() //Select * From Students
                                .Where(x => x.GroupFK == selectedGroupCode) //Select * From Students Where GroupFK = groupCode
                                .OrderBy(x => x.IdCard)  //Select * From Students Where GroupFK = groupCode order by FirstName
                                .ToList(); //here is where the execution i.e. opening a connection to db actually happens

                myModel.Attendances = _attendancesRepository.GetAttendances().Where(x => x.SubjectFK == selectedSubjectCode
                && x.Timestamp.Day == date.Day
                && x.Timestamp.Month == date.Month
                && x.Timestamp.Year == date.Year
                && x.Timestamp.Hour == date.Hour
                && x.Timestamp.Minute == date.Minute //to exclude the seconds and milliseconds
                && x.Student.GroupFK == selectedGroupCode
                ).OrderBy(x => x.Student.IdCard).ToList();   

                ViewBag.update = "true";

                return View(myModel);
            }

        }


        [HttpPost] //it saves the absents and presents of all the students from the first Create method
        public IActionResult Create(List<Attendance> attendances, bool update)
        {
            if(attendances.Count >0)
            {

                if (update)
                {
                    _attendancesRepository.UpdateAttendances(attendances);
                }
                else
                {
                    _attendancesRepository.AddAttendances(attendances);
                }

               
                TempData["message"] = "Attendance saved";
            }

            return RedirectToAction("Index");
        }
    }
}

