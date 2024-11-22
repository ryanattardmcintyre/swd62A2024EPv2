using DataAccess.DataContext;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class SubjectsRepository
    {
        private AttendanceContext myContext;

        //Constructor Injection
        public SubjectsRepository(AttendanceContext _myContext)
        {
            myContext = _myContext;
        }

        public IQueryable<Subject> GetSubjects()
        {
            return myContext.Subjects;
        }


        //Home exercise
        //note: to practice at home, you implement all the CRUD functions
        //note: if you would like to consume the C U D, repeat the steps we applied for Student entity
        //note: after the above...implement a SubjectsController ...Index, Create, Update, Delete

    }
}
