using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 框架搭建.Filter;

namespace 框架搭建
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(option => {
                option.Filters.Add<CustomExceptionFilterAttribute>();//全局捕捉异常；
            });
            services.AddHttpContextAccessor();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            #region 捕捉异常的补充 
            app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");//只要不是200 都能进来
            //如果请求响应Code 不是200,就把他变成200,同时组建一个页面响应给客户端；
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "text/html";
                    await context.Response.WriteAsync("<html lang=\"en\"><body>\r\n");
                    await context.Response.WriteAsync("ERROR!<br><br>\r\n");
                    var exceptionHandlerPathFeature =
                        context.Features.Get<IExceptionHandlerPathFeature>();

                    Console.WriteLine("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");
                    Console.WriteLine($"{exceptionHandlerPathFeature?.Error.Message}");
                    Console.WriteLine("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");

                    //if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
                    //{
                    await context.Response.WriteAsync($"File error thrown!<br>{exceptionHandlerPathFeature?.Error.Message}<br>\r\n");
                    //}
                    await context.Response.WriteAsync("<a href=\"/\">Home</a><br>\r\n");
                    await context.Response.WriteAsync("</body></html>\r\n");
                    await context.Response.WriteAsync(new string(' ', 512)); // IE padding
                });
            });
            #endregion
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Homepage}/{action=Index}/{id?}");
            });
        }
    }
}
