using DataAccess.DataContext;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class AttendancesRepository
    {
        private AttendanceContext _attendanceContext;
        public AttendancesRepository(AttendanceContext attendanceContext) {
            _attendanceContext = attendanceContext;
        }

        public void AddAttendance(Attendance a)
        {
            a.Timestamp = DateTime.Now;

            _attendanceContext.Attendances.Add(a);
            _attendanceContext.SaveChanges();
        }

        public IQueryable<Attendance> GetAttendances(DateTime date, string groupCode, string subjectCode)
        {

        }



    }
}
