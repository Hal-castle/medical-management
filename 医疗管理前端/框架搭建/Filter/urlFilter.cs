using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using 框架搭建.Controllers;

namespace 框架搭建.Filter
{
    /// <summary>
    /// 授权过滤器
    /// </summary>
    public class urlFilter : Attribute, IResourceFilter
    {
        private Dictionary<int, string> CustomCacheDictionary = HomePageController.urlAdr;
        //public urlFilter(Dictionary<int, string>  _CustomCacheDictionary)
        //{
        //    CustomCacheDictionary = _CustomCacheDictionary;
        //}
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            string key = context.HttpContext.Request.Path;
            if (!CustomCacheDictionary.ContainsValue(key))
            {
                //object result = "";//就应该直接响应给客户端，就不用继续往后自行；
                //需要一个短路器
                //context.Result = result as IActionResult;//只要是Result赋值，就不再继续往后执行了；
                context.Result = new RedirectToActionResult("Error", "Shared", null);
            }
            //Console.WriteLine("this is CustomResourceFilterAttribute.OnResourceExecuting");
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            ////能执行到这儿来，肯定是已经执行完了 Action，该做的计算肯定也都已经完成；在这里就有计算的结果
            ////就应该把结果保存到缓存中去；
            //string key = context.HttpContext.Request.Path;
            //CustomCacheDictionary[key] = context.Result;
            //Console.WriteLine("this is CustomResourceFilterAttribute.OnResourceExecuted");
        }
    }
}

