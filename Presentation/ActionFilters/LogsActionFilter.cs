using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.ActionFilters
{
    public class LogsActionFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Log myLog = new Log();
            myLog.Message = $"Timestamp:{DateTime.Now}, Action: {context.HttpContext.Request.Path}, " +
                $"Querystring: {context.HttpContext.Request.QueryString}";


            myLog.User = "Anonymous user";
            if (context.HttpContext.User != null)
            {
                if (context.HttpContext.User.Identity.IsAuthenticated)
                {
                    myLog.User = context.HttpContext.User.Identity.Name; //email address
                }
            }


            myLog.IpAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString(); //[::1] 

            LogsRepository logsRepository = context.HttpContext.RequestServices.GetService<LogsRepository>();
            logsRepository.AddLog(myLog);


            base.OnActionExecuting(context); //if you want to keep running the next code smoothly don't delete this line
        }
    }
}
