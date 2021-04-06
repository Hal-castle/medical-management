using AuthenticationCenter.Utility;
using Command;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationCenter.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly ILogger<WeatherForecastController> _logger;
        private ICustomJWTService _iJWTService = null;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, ICustomJWTService service)
        {
            _logger = logger;
            _iJWTService = service;
        }
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="name">账号</param>
        /// <param name="password">密码</param>
        /// <returns>JSON字符串(是否成功，token字符串)</returns>
        [HttpPost]
        public  string Login(Tb_User u)
        
        {
            if (string.IsNullOrWhiteSpace(u.User_Password) ||string.IsNullOrWhiteSpace(u.User_Account))
                return JsonConvert.SerializeObject(new
                {
                    result = false,
                    token = ""
                });
            if (new DbContext<Tb_User>().GetSingle(s=>s.User_Account.Equals(u.User_Account)&s.User_Password.Equals(u.User_Password)) !=null)//实际情况下应该到数据库中做验证
            {
                //就应该生成Token 
                string token = this._iJWTService.GetToken(u.User_Account, u.User_Password);
                log(u.User_Account);
                return JsonConvert.SerializeObject(new
                {
                    result = true,
                    token,
                    User = new {Account=u.User_Account,Password=u.User_Password }
                }); ;
            }
            else
            {
                return JsonConvert.SerializeObject(new
                {
                    result = false,
                    token = ""
                });
            }
        }
        /// <summary>
        /// 登录成功后写入日志
        /// </summary>
        /// <param name="user"></param>
        private void log(string user)
        {
            string str3 = Directory.GetCurrentDirectory();//获取应用程序的当前工作目录。
            string FilePath = Path.Combine(str3, "Log on to log.txt");//Log on to log.txt
            if (!System.IO.File.Exists(FilePath))
            {
                System.IO.FileStream f = System.IO.File.Create(FilePath);
                f.Close();
                f.Dispose();
            }
            string str = "登录用户：" + user  + "\t登录时间：" + DateTime.Now + "\n";
            System.IO.StreamWriter f2 = new System.IO.StreamWriter(FilePath, true, System.Text.Encoding.UTF8);
            f2.WriteLine(str);
            f2.Close();
            f2.Dispose();
        }
        //[Authorize]
        /// <summary>
        /// 登录成功后获取菜单
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetMenu([FromBody]Tb_User u)
        {
           var a= new DbContext<object>().SqlQueryList($"select e.Jur_Id,e.Jur_icon,b.User_Account,b.User_Password,b.User_photo,c.Role_Name,c.Role_Describe,e.Jur_Name,e.Jur_superior,e.Jur_Url,e.Jur_describe from User_Role a join tb_User b on a.Use_Id=b.Use_Id join Tb_Role c on c.Role_Id=a.Rol_Id join Role_Jurisdiction d on d.Rol_Id=c.Role_Id join Jurisdiction e on e.Jur_Id=d.Jur_Id where User_Account='{u.User_Account}' and User_Password='{u.User_Password}'");
            return a;
        }
    }
}
