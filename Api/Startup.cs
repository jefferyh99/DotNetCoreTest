using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.注册服务
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            services.AddMvcCore()
           .AddAuthorization()//authentication services to DI ,注入服务
           .AddJsonFormatters();

            //API验证Token方式,使用.net core 默认验证方式
            //使用原始方式
            //services.AddAuthentication("Bearer")
            //    .AddJwtBearer("Bearer", options =>
            //    {
            //        options.Authority = "http://localhost:5000";
            //        options.RequireHttpsMetadata = false;

            //        options.Audience = "api1";//对应ApiResource Name，需要获取访问的资源名字，验证颁发的token是否有效

            //        //每隔多久需要验证一下token,jwt不会每次都验证下token的
            //        options.TokenValidationParameters.ClockSkew = TimeSpan.FromMinutes(1);

            //        //options.ClaimsIssuer,代表这个JWT的签发主体（who created and signed this token）
            //        //options.Audience,代表这个JWT的接收对象，接收token的对象，且需要验证是否合法（who and what the token is intended for）,这个token用于什么地方，用于访问api1这个resource
            //    });

            //使用库方式

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;

                    options.ApiName = "api1";//对应ApiResource Name，需要获取访问的资源名字，验证颁发的token是否有效
                    options.ApiSecret = "api1 secret";//需要身份验证时，就需要提供Secret

                });


            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins("http://localhost:5003")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.使用服务
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("default");

            //验证服务
            app.UseAuthentication();//adds the authentication middleware to the pipeline，使用服务

            app.UseMvc();
        }
    }
}
