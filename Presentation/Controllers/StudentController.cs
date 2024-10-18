using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{

    //as a good practice of how we are structuing our architecture:

    //keep the repository classes to interact directly with the database;
    //keep the controllers to handle requests/responses i.e. user input and then sanitaze accordingly
    //in other words do not make any calls directly to the database in the controller

    public class StudentController : Controller
    {

        private StudentsRepository _studentRepository;

        //Construtor Injection
        public StudentController(StudentsRepository studentsRepository) {

            _studentRepository = studentsRepository;
        }

        public IActionResult List()
        {
            var list = _studentRepository.GetStudents();
            return View(list); //we are passing into the View the list containing the fetched students
        }

    }
}
