using API.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;

namespace API
{
    public class Startup
    {
        private readonly string Any = "Any"; //跨域
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(m => m.AddPolicy(Any, a => a.SetIsOriginAllowed(_ => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials()));//跨域
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });
            #region RSA
            {
                // 读取公钥
                string path = Path.Combine(Directory.GetCurrentDirectory(), "key.public.json");
                string key = File.ReadAllText(path);//this.Configuration["SecurityKey"];
                Console.WriteLine($"KeyPath:{path}");

                var keyParams = JsonConvert.DeserializeObject<RSAParameters>(key);
                //SigningCredentials credentials = new SigningCredentials(new RsaSecurityKey(keyParams), SecurityAlgorithms.RsaSha256Signature);


                JWTTokenOptions tokenOptions = new JWTTokenOptions();
                Configuration.Bind("JWTTokenOptions", tokenOptions);

                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,//是否验证Issuer
                        ValidateAudience = true,//是否验证Audience
                        ValidateLifetime = true,//是否验证失效时间
                        ValidateIssuerSigningKey = true,//是否验证SecurityKey
                        ValidAudience = tokenOptions.Audience,//Audience
                        ValidIssuer = tokenOptions.Issuer,//Issuer，这两项和前面签发jwt的设置一致
                        IssuerSigningKey = new RsaSecurityKey(keyParams),
                        //IssuerSigningKeyValidator = (m, n, z) =>
                        // {
                        //     Console.WriteLine("This is IssuerValidator");
                        //     return true;
                        // },
                        //IssuerValidator = (m, n, z) =>
                        // {
                        //     Console.WriteLine("This is IssuerValidator");
                        //     return "http://localhost:5726";
                        // },
                        //AudienceValidator = (m, n, z) =>
                        //{
                        //    Console.WriteLine("This is AudienceValidator");
                        //    return true;
                        //    //return m != null && m.FirstOrDefault().Equals(this.Configuration["Audience"]);
                        //},//自定义校验规则，可以新登录后将之前的无效
                    };
                });
            }
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();
            //第一步：启用鉴权授权
            app.UseAuthentication();//注意添加这一句，启用验证

            app.UseRouting();

            app.UseCors(Any);// 跨域

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
