using DataAccess.DataContext;
using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class AttendanceController : Controller
    {
        AttendancesRepository _attendancesRepository;
        public AttendanceController(AttendancesRepository attendancesRepository) {
        _attendancesRepository= attendancesRepository;
        }

      

   
        //a page which shows me which attendances i can take 
        public IActionResult Index()
        {

             //note: getting all attendances, but then extracting some information which i will put on screen
             //      being Subject, Date, Group
             //... this requires me to create a ViewModel to accomodate this newly formed group of data

            //a history of attendances
               var groupedAttendances = _attendancesRepository.GetAttendances()
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

            //to do:
            //we need to pass to the page a list of Groups, a list of Subjects we have
            //GroupsRepository >> GetGroups
            //SubjectsRepository >> GetSubjects
            
            //create a new ViewModel which accepts and also a list of Groups and a list of Subjects


            return View(groupedAttendances);
        }

        [HttpGet] //it needs to show me which students are supposed to be in that attendance list
        public IActionResult Create(string groupCode, string subjectCode)
        {
                //to do:
                //a list of students pertaining to the group
                //group code
                //subject code/name

            
        
        }


        [HttpPost] //it saves the absents and presents of all the students from the first Create method
        public IActionResult Create()
        { }
    }
}
