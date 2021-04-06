using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 框架搭建.Filter;

namespace 框架搭建.Controllers
{
    public class HomePageController : Controller
    {
        //存储登录信息
        public static Dictionary<int, string> urlAdr = new Dictionary<int, string>();
        public IActionResult Students()
        {
            return View();
        }
        //登录页面
        public IActionResult Login()
        {
            return View();
        }
        //主页
        public IActionResult Index()
        {
            return View();
        }
        //总览
        [urlFilter]
        public IActionResult Pandect_management()
        {
            return View();
        }
        //患者管理
        [urlFilter]
        public IActionResult Patient_management()
        {
            return View();
        }
        //医生管理
        [urlFilter]
        public IActionResult Doctor_management()
        {
            return View();
        }
        //出诊管理
        [urlFilter]
        public IActionResult Visits_to_manage()
        {
            return View();
        }
        //支付管理
        [urlFilter]
        public IActionResult Payment_management()
        {
            return View();
        }
        //病房分配
        [urlFilter]
        public IActionResult Ward_distribution()
        {
            return View();
        }
        public IActionResult qwer()
        {
            return View();
        }
        /// <summary>
        /// 图片上传并存入数据库
        /// </summary>
        /// <param name="formData">从页面上传的文件</param>
        /// <returns></returns>

        /// <summary>
        /// 图片上传并存入数据库
        /// </summary>
        /// <returns></returns>
        public object InsertPicture([FromForm] IFormCollection formData)
        {
            IFormFile uploadfile = formData.Files[0];
            if (uploadfile != null)
            {
                //文件后缀
                var fileExtension = Path.GetExtension(uploadfile.FileName);
                var strDateTime = DateTime.Now.ToString("yyMMddhhmmssfff"); //取得时间字符串
                var strRan = Convert.ToString(new Random().Next(100, 999)); //生成三位随机数
                var saveName = strDateTime + strRan + fileExtension;
                var path = "Img";
                var di = ("/" + path + "/" + saveName);
                var bi = Path.Combine("wwwroot", path);
                if (!Directory.Exists(bi))
                {
                    Directory.CreateDirectory(bi);
                }
                using (FileStream fs = System.IO.File.Create(Path.Combine(bi, saveName)))
                {
                    uploadfile.CopyTo(fs);
                    fs.Flush();
                }
                return new { code = 0, path = di };
            }
            return new { code = 1 };
        }
        /// <summary>
        /// 传递登录信息
        /// </summary>
        /// <param name="URL"></param>
        public void Save(string URL)
        {
            urlAdr.Clear();
            urlAdr.Add(0, "/HomePage/Index");
            urlAdr.Add(-1, "/HomePage/Pandect_management");
            string[] arr = URL.Substring(1).Split(",");
            for (int i = 0; i < arr.Length; i++)
            {
                urlAdr.Add(i + 1, arr[i]);
            }
        }
    }
}
