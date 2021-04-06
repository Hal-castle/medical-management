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
        private readonly string Any = "Any"; //����
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(m => m.AddPolicy(Any, a => a.SetIsOriginAllowed(_ => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials()));//����
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });
            #region RSA
            {
                // ��ȡ��Կ
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
                        ValidateIssuer = true,//�Ƿ���֤Issuer
                        ValidateAudience = true,//�Ƿ���֤Audience
                        ValidateLifetime = true,//�Ƿ���֤ʧЧʱ��
                        ValidateIssuerSigningKey = true,//�Ƿ���֤SecurityKey
                        ValidAudience = tokenOptions.Audience,//Audience
                        ValidIssuer = tokenOptions.Issuer,//Issuer���������ǰ��ǩ��jwt������һ��
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
                        //},//�Զ���У����򣬿����µ�¼��֮ǰ����Ч
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
            //��һ�������ü�Ȩ��Ȩ
            app.UseAuthentication();//ע�������һ�䣬������֤

            app.UseRouting();

            app.UseCors(Any);// ����

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
