using API.Utility;
using Command;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlSugar;
using System.Linq.Expressions;
namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region 用户
        [HttpGet]
        //[CustomResourceFilterAttribute]
        //[SignValidateAttribute]
        public object GetUser(int page = 1, int limit = 3)
        {
            var p = new PageModel() { PageIndex = page, PageSize = limit };// 分页查询
            var data = new DbContext<Tb_User>().GetPageList(s => true, p, null, OrderByType.Asc);
            int total = p.PageCount;//返回总数
            return new { result = data, total = total };
        }
        [HttpPost]
        public object AddUser([FromBody] Tb_User student)
        {
            if (new DbContext<Tb_User>().GetSingle(s => s.User_Account.Equals(student.User_Account)) != null)
            {
                return new { code = 1 };
            }
            else
            {
                if (new DbContext<Tb_User>().Insert(student))
                {
                    return new { code = 2 };
                }
                else
                {
                    return new { code = 0 };
                }
            }
        }
        [HttpDelete]
        public bool DelUser(string id)
        {
            string[] str = id.Substring(1).Split(",");
            foreach (var item in str)
            {
                new DbContext<User_Role>().Delete(s => s.Use_Id.Equals(item));
            }
            return new DbContext<Tb_User>().Delete(str);
        }
        [HttpPut]
        public object PutUser([FromBody] Tb_User stu)
        {
            if (new DbContext<Tb_User>().GetSingle(s => s.User_Account.Equals(stu.User_Account)) != null)
            {
                return new { code = 1 };
            }
            else
            {
                if (new DbContext<Tb_User>().Update(stu))
                {
                    return new { code = 2 };
                }
                else
                {
                    return new { code = 0 };
                }
            }
        }
        #endregion



        #region 角色
        [HttpGet]
        //[CustomResourceFilterAttribute]
        //[SignValidateAttribute]
        public object GetRole(int page = 1, int limit = 3)
        {
            var p = new PageModel() { PageIndex = page, PageSize = limit };// 分页查询
            var data = new DbContext<Tb_Role>().GetPageList(s => true, p,null,OrderByType.Asc);
            int total = p.PageCount;//返回总数
            return new { result = data, total = total };
        }
        [HttpPost]
        public object AddRole([FromBody] Tb_Role student)
        {
            if(new DbContext<Tb_Role>().GetSingle(s=>s.Role_Name.Equals(student.Role_Name)) != null)
            {
                return new { code = 1 };
            }
            else 
            {
                if (new DbContext<Tb_Role>().Insert(student))
                {
                    return new { code = 2 };
                }
                else
                {
                    return new { code = 0 };
                }
            }
        }
        [HttpDelete]
        public   bool DelRole(string id)
        {
            string[] str = id.Substring(1).Split(",");
            foreach (var item in str)
            {
                 new DbContext<User_Role>().Delete(s => s.Rol_Id.Equals(item));
                 new DbContext<Role_Jurisdiction>().Delete(s => s.Rol_Id.Equals(item));
            }
            return new DbContext<Tb_Role>().Delete(str);
        }
        [HttpPut]
        public object PutRole([FromBody] Tb_Role stu)
        {
            if (new DbContext<Tb_Role>().GetSingle(s => s.Role_Name.Equals(stu.Role_Name)) != null)
            {
                return new { code = 1 };
            }
            else
            {
                if (new DbContext<Tb_Role>().Update(stu))
                {
                    return new { code = 2 };
                }
                else
                {
                    return new { code = 0 };
                }
            }
        }
        /// <summary>
        /// 分配用户给某个角色
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        [HttpDelete]
        public bool RoleAddUser(string id)
        {
            string[] data = id.Split("-");
            string userId = data[0];
            string[] str = data[1].Substring(1).Split(",");
            List<User_Role> a = new List<User_Role>();
            User_Role b = null;
            foreach (var item in str)
            {
                b = new User_Role()
                {
                    Rol_Id = Convert.ToInt32(userId),
                    Use_Id = Convert.ToInt32(item),
                };
                a.Add(b);
            }
            return new DbContext<User_Role>().Insert(a);
        }
        /// <summary>
        /// 分配权限给某个角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public bool RoleAddJur(string id)
        {
            string[] data = id.Split("-");
            string roleId = data[0];
            string[] str = data[1].Substring(1).Split(",");
            List<Role_Jurisdiction> a = new List<Role_Jurisdiction>();
            Role_Jurisdiction b = null;
            foreach (var item in str)
            {
                b = new Role_Jurisdiction()
                {
                    Rol_Id = Convert.ToInt32(roleId),
                    Jur_Id = Convert.ToInt32(item),
                };
                a.Add(b);
            }
            return new DbContext<Role_Jurisdiction>().Insert(a);
        }
        //显示所有权限
        [HttpGet]
        public object GetRole_Jur(int RoleId)
        {
            var a = new DbContext<Role_Jurisdiction>().GetList(s => s.Rol_Id.Equals(RoleId));
            var data = new DbContext<Jurisdiction>().GetList();
            var query = from s in data
                        join d in a
                        on s.Jur_Id equals d.Jur_Id
                        select s;
            return new { result = query };
        }
        //显示所有角色
        [HttpGet]
        public object GetRole_User(int RoleId)
        {
            var a = new DbContext<User_Role>().GetList(s => s.Rol_Id.Equals(RoleId));
            var data = new DbContext<Tb_User>().GetList();
            var query=from  s in data 
                      join d in a
                      on s.Use_Id equals d.Use_Id
                      select s;
            return new { result = query };
        }
        #endregion



        #region 权限
        [HttpGet]
        //[CustomResourceFilterAttribute]
        //[SignValidateAttribute]
        public object GetJur(int page = 1, int limit = 3)
        {
            var p = new PageModel() { PageIndex = page, PageSize = limit };// 分页查询
            var data = new DbContext<Jurisdiction>().GetPageList(s => true, p, null, OrderByType.Asc);
            int total = p.PageCount;//返回总数
            return new { result = data, total = total };
        }
        [HttpPost]
        public object AddJur([FromBody] Jurisdiction student)
        {
            if (new DbContext<Jurisdiction>().GetSingle(s => s.Jur_Name.Equals(student.Jur_Name) || s.Jur_describe.Equals(student.Jur_describe)) != null)
            {
                return new { code = 1 };
            }
            else
            {
                if (new DbContext<Jurisdiction>().Insert(student))
                {
                    return new { code = 2 };
                }
                else
                {
                    return new { code = 0 };
                }
            }
        }
        [HttpDelete]
        public bool DelJur(string id)
        {
            string[] str = id.Substring(1).Split(",");
            foreach (var item in str)
            {
                new DbContext<Role_Jurisdiction>().Delete(s => s.Jur_Id.Equals(item));
            }
            return new DbContext<Jurisdiction>().Delete(str);
        }
        [HttpPut]
        public object PutJur([FromBody] Jurisdiction stu)
        {
            if (new DbContext<Jurisdiction>().GetSingle(s => s.Jur_Name.Equals(stu.Jur_Name) || s.Jur_describe.Equals(stu.Jur_describe)) != null)
            {
                return new { code = 1 };
            }
            else
            {
                if (new DbContext<Jurisdiction>().Update(stu))
                {
                    return new { code = 2 };
                }
                else
                {
                    return new { code = 0 };
                }
            }
        }
        #endregion

    }
}
