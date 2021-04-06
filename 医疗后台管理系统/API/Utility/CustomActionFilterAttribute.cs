using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Utility
{
    /// <summary>
    /// 日志
    /// </summary>
    public class CustomActionFilterAttribute : Attribute, IActionFilter
    {
        private ILogger<CustomActionFilterAttribute> _Logger = null;

        public CustomActionFilterAttribute(ILogger<CustomActionFilterAttribute> logger)
        {
            _Logger = logger;
            _Logger.LogInformation("日志现在创建了");
        }



        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("this is CustomActionFilterAttribute.OnActionExecuting");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("this is CustomActionFilterAttribute.OnActionExecuted");
        }


    }
}
