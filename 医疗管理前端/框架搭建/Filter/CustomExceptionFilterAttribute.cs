using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace 框架搭建.Filter
{
    /// <summary>
    /// 1.自定一个CustomExceptionFilterAttribute，继承Attribute，实现IExceptionoFilter接口
    /// 2.标记在某Action上
    /// 3.Action发生异常---就会触发CustomExceptionFilterAttribute。OnException方法  ---其实就是捕捉到了异常；
    /// 4.就应该异常处理；
    /// </summary>
    public class CustomExceptionFilterAttribute : Attribute, IExceptionFilter
    {

        private IModelMetadataProvider _modelMetadataProvider = null;

        public CustomExceptionFilterAttribute(IModelMetadataProvider modelMetadataProvider)
        {
            _modelMetadataProvider = modelMetadataProvider;
        }
        /// <summary>
        /// 就是在异常的时候触发这个方法
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled) //1.标识当前捕捉的这个异常没有被处理掉
            {
                if (IsAjaxRequest(context.HttpContext.Request))
                {
                    //这里就应该准备一个Json格式的数据响应回去
                    context.Result = new JsonResult(new
                    {
                        Result = false,
                        Msg = context.Exception.Message
                    });//中断式---请求到这里结束了，不再继续Action
                }
                else //2.就应该在这里进行异常处理 
                {
                    //如果处理异常：一般就是把异常消息打印出来然后给一个友好的页面告诉用户；说系统异常了，请联系工作人员；---系统发生异常后，异常有人管；
                    var result = new ViewResult { ViewName = "~/Views/Shared/Error.cshtml" };
                    result.ViewData = new ViewDataDictionary(_modelMetadataProvider, context.ModelState);
                    result.ViewData.Add("Exception", context.Exception);
                    context.Result = result; //断路器---只要对Result赋值--就不继续往后了； 
                }
                //如果是Ajax请求呢？我们应该返回一个Json格式数据给请求方；


            }
            //3.处理完毕以后，应该标记为这个异常被处理过了
            context.ExceptionHandled = true;
        }

        private bool IsAjaxRequest(HttpRequest request)
        {
            string header = request.Headers["X-Requested-With"];
            return "XMLHttpRequest".Equals(header);
        }
    }
}
