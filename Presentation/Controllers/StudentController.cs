using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

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

        [HttpGet]
        public IActionResult List()
        {
            var list = _studentRepository.GetStudents();
            return View(list); //we are passing into the View the list containing the fetched students
        }

        //handle the redirection from the List > to the Edit page
        //the first one is going to load the existent details for the end user to see in the textboxes
        [HttpGet]
        public IActionResult Edit(string id)
        {
            //is where you need to implement fetching of the Groups
            //and creation of a ViewModel
            var student = _studentRepository.GetStudent(id);
            if (student == null)
            {
                return RedirectToAction("List");
            }
            else
            {
                return View(student);
            }
        }


        //handle the click of the Submit Changes button
        [HttpPost]
        public IActionResult Update(Student student, IFormFile file, [FromServices] GroupsRepository groupRepository, [FromServices] IWebHostEnvironment host) {

            try
            {
                if (student == null)
                {
                    //Session - keeps the data in scope on the server-side only (so across controllers, etc)
                    //ViewData - keeps the data  in scope between controller and View for 1 response only NOT a redirection
                    //ViewBag - exactly like ViewData however it allows to create variables on the fly
                    //TempData - this is like ViewData but the data inside it survives one redirection

                    TempData["error"] = "Id card no supplied does not exist";

                    return Redirect("Error");
                }
                else
                {
                    //validations, sanitization of data

                    ModelState.Remove(nameof(Student.Group));
                    ModelState.Remove("file");

                    //this line will ensure that if there are validation policies (Centralized /not)
                    //applied, they will have to pass from here; it ensures that validations have been triggered
                    if (ModelState.IsValid)
                    {

                        //file upload
                        if (file != null)
                        {
                            string filename = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);

                            string pathObtainedFromCurrentDirectory = Directory.GetCurrentDirectory();

                            //C:\Users\attar\source\repos\swd62A2024EP\swd62A2024EP\Presentation\wwwroot
                            string absolutePath = host.WebRootPath + "\\images\\" + filename;


                            using (var f = System.IO.File.Create(absolutePath))
                            {
                                file.CopyTo(f);
                            }

                            string relativePath = "\\images\\" + filename;
                            student.ImagePath = relativePath;

                        }


                        _studentRepository.UpdateStudent(student);
                        TempData["message"] = "Student was added successfully";

                        return RedirectToAction("List");
                    }

                    //add some error messages here
                    TempData["error"] = "Check your inputs";
                    return View("Edit",student); //will be looking for a view Update
                }
            }
            catch
            {
                TempData["error"] = "Something went wrong. We are working on it";
                return Redirect("Error");
            }
        }

        [HttpGet] //used to load the page with empty textboxes
        public IActionResult Create([FromServices] GroupsRepository groupRepository) {

            //eventually: we need to fetch a list of existing groups the end user can select from

            var myGroups = groupRepository.GetGroups();

            //How are we going to pass the myGroups into the View?
            //Approach 1 - we can pass a model into the View where we create a ViewModel
            //problem is: you cannot pass IQueryable<Group> model into Student model
            StudentCreateViewModel myModel = new StudentCreateViewModel();
            myModel.Groups = myGroups.ToList();
        //  myModel.Student = new Student();
            
            return View(myModel); 

            //Approach 2
        }

        [HttpPost] //is triggered by the submit button of the form
        public IActionResult Create(Student s, IFormFile file, [FromServices] GroupsRepository groupRepository, [FromServices] IWebHostEnvironment host) {

            if (_studentRepository.GetStudent(s.IdCard) != null)
            {
                TempData["error"] = "Student already exists";
                return RedirectToAction("List");
            }
            else
            {
                ModelState.Remove(nameof(Student.Group));
                ModelState.Remove("file");

                //this line will ensure that if there are validation policies (Centralized /not)
                //applied, they will have to pass from here; it ensures that validations have been triggered
                if (ModelState.IsValid)
                {
                    //file upload
                    if (file != null)
                    {
                        string filename = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);

                        string pathObtainedFromCurrentDirectory = Directory.GetCurrentDirectory();

                        //C:\Users\attar\source\repos\swd62A2024EP\swd62A2024EP\Presentation\wwwroot
                        string absolutePath = host.WebRootPath +"\\images\\" + filename;


                        using (var f = System.IO.File.Create(absolutePath))
                        {
                            file.CopyTo(f);
                        }

                        string relativePath = "\\images\\" + filename;
                        s.ImagePath = relativePath;

                    }


                    //save the details in the db
                    _studentRepository.AddStudent(s);
                    TempData["message"] = "Student was added successfully";

                    return RedirectToAction("List");
                }

                //add some error messages here
                TempData["error"] = "Check your inputs";

                //populating a StudentCreateViewModel
                var myGroups = groupRepository.GetGroups();
                StudentCreateViewModel myModel = new StudentCreateViewModel();
                myModel.Groups = myGroups.ToList();
                myModel.Student = s; //why do i assign Student s that was submitted in this method?
                                    //passing the same instance back to the page
                                    //so that I show the end-user the same data he/she gave me

                return View(myModel); //will be looking for a view as the action name.....Create
            }
        }

    }
}
