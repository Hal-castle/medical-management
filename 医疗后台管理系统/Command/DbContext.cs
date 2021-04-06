using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.IO;
using System.Reflection;
using Models;


namespace Command
{
    public class DbContext<T> where T : class, new()
    {
        public readonly static IConfiguration Configuration = new ConfigurationBuilder()
                       .SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json", optional: true)
                       .Build();

        public DbContext()
        {
            Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = Configuration.GetConnectionString("DB").ToString(),//连接符字串
            DbType = (SqlSugar.DbType)Enum.Parse(typeof(SqlSugar.DbType), Configuration.GetConnectionString("Type").ToString()),//数据库类型
            InitKeyType = InitKeyType.Attribute,//从特性读取主键和自增列信息
                IsAutoCloseConnection = true,//开启自动释放模式和EF原理一样我就不多解释了

            });;
        }
        public void CreateTable(bool Backup = false, int StringDefaultLength = 50, params Type[] types)
        {
            Db.CodeFirst.SetStringDefaultLength(StringDefaultLength);
            Db.DbMaintenance.CreateDatabase();
            if (Backup)
            {
                Db.CodeFirst.BackupTable().InitTables(types);
            }
            else
            {
                Db.CodeFirst.InitTables(types);
            }
        }
        /*
         人员管理*/
    
        //注意：不能写成静态的
        public SqlSugarClient Db;//用来处理事务多表查询和复杂的操作

        public SimpleClient<T> CurrentDb { get { return new SimpleClient<T>(Db); } }//用来处理T表的常用操作

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public virtual T GetSingle(Expression<Func<T, bool>> whereExpression)
        {
           return CurrentDb.GetSingle(whereExpression);
        }
        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        public virtual List<T> GetList()
        {
            return CurrentDb.GetList();
        }

        /// <summary>
        /// 根据表达式查询
        /// </summary>
        /// <returns></returns>
        public virtual List<T> GetList(Expression<Func<T, bool>> whereExpression)
        {
            return CurrentDb.GetList(whereExpression);
        }


        /// <summary>
        /// 根据表达式查询分页
        /// </summary>
        /// <returns></returns>
        public virtual List<T> GetPageList(Expression<Func<T, bool>> whereExpression, PageModel pageModel)
        {
            return CurrentDb.GetPageList(whereExpression, pageModel);
        }

        /// <summary>
        /// 根据表达式查询分页并排序
        /// </summary>
        /// <param name="whereExpression">it</param>
        /// <param name="pageModel"></param>
        /// <param name="orderByExpression">it=>it.id或者it=>new{it.id,it.name}</param>
        /// <param name="orderByType">OrderByType.Desc</param>
        /// <returns></returns>
        public virtual List<T> GetPageList(Expression<Func<T, bool>> whereExpression, PageModel pageModel, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc)
        {
            return CurrentDb.GetPageList(whereExpression, pageModel, orderByExpression, orderByType);
        }
        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <returns></returns>
        public virtual T GetById(dynamic id)
        {
            return CurrentDb.GetById(id);
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Delete(dynamic id)
        {
            return CurrentDb.Delete(id);
        }


        /// <summary>
        /// 根据实体删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Delete(T data)
        {
            return CurrentDb.Delete(data);
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Delete(dynamic[] ids)
        {
            return CurrentDb.AsDeleteable().In(ids).ExecuteCommand() > 0;
        }
        public void D(int a)
        {
            Db.Deleteable<User_Role>().Where(p => p.Rol_Id == SqlFunc.Subqueryable<Tb_Role>().Where(s => s.Role_Id == p.Rol_Id).Select(s => s.Role_Id)).ExecuteCommand();
        }
        /// <summary>
        /// 根据表达式删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Delete(Expression<Func<T, bool>> whereExpression)
        {
            return CurrentDb.Delete(whereExpression);
        }


        /// <summary>
        /// 根据实体更新，实体需要有主键
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Update(T obj)
        {
            return CurrentDb.Update(obj);
        }

        /// <summary>
        ///批量更新
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Update(List<T> objs)
        {
            return CurrentDb.UpdateRange(objs);
        }

        //
        public virtual bool UpdataRange(Expression<Func<T,T>> fun1,Expression<Func<T,bool>> fun2)
        {
            return CurrentDb.Update(fun1,fun2);
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Insert(T obj)
        {
            return CurrentDb.Insert(obj);
        }


        /// <summary>
        /// 批量
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Insert(List<T> objs)
        {
            return CurrentDb.InsertRange(objs);
        }

        /// <summary>
        /// 查询sql 返回list
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public List<T> SqlQueryList(string sqlstr)
        {
            return this.Db.SqlQueryable<T>(sqlstr).ToList();
        }
        /// <summary>
        /// 查询sql 返回table
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public DataTable SqlQueryTab(string sqlstr)
        {
            return this.Db.Ado.GetDataTable(sqlstr);
        }
    }
}