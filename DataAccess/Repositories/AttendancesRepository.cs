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
        //CRUD 
        public void AddAttendance(Attendance a)
        {
            a.Timestamp = DateTime.Now;

            _attendanceContext.Attendances.Add(a);
            _attendanceContext.SaveChanges();
        }

        public void AddAttendances(List<Attendance> attendances)
        {
            var currentTime = DateTime.Now; //time is taken once

            foreach (var a in attendances)
            {
                a.Timestamp = currentTime; //meaning all the records are going to get the same exact time including the milliseconds
                _attendanceContext.Attendances.Add(a);
            }

            _attendanceContext.SaveChanges(); //call this once at the end. this will refrain from opening a connection to the database
                                              //multiple times
        }



        public IQueryable<Attendance> GetAttendances(DateTime date, string groupCode, string subjectCode)
        {
            return _attendanceContext.Attendances.Where(x =>
            x.Timestamp.Day == date.Day && x.Timestamp.Year == date.Year && x.Timestamp.Month == date.Month
            && x.SubjectFK == subjectCode
            && x.Student.GroupFK == groupCode
            );
        }

        public IQueryable<Attendance> GetAttendances()
        {
            return _attendanceContext.Attendances;
        }

        public void UpdateAttendances(List<Attendance> attendances)
        {
            foreach (var a in attendances)
            {
                var oldRecord = GetAttendances().SingleOrDefault(x => x.Id == a.Id);
                oldRecord.Present = a.Present;
            }
            _attendanceContext.SaveChanges();
        }



    }
}
