using DataAccess.DataContext;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    /// <summary>
    /// logs data into the database
    /// </summary>
    public class LogsDbRepository: ILogsRepository
    {
        private AttendanceContext myContext;

        //Constructor Injection
        public LogsDbRepository(AttendanceContext _myContext)
        {
            myContext = _myContext;
        }
        public void AddLog(Log myLog)
        {
            myContext.Logs.Add(myLog);
            myContext.SaveChanges();
        }

        public IQueryable<Log> LoadLogs()
        {
            return myContext.Logs;
        }
    }
}
