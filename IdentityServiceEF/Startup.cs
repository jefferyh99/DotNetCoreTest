using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServiceEF
{
    public class Startup
    {
        //添加默认QuickStart-UI
        //iex ((New-Object System.Net.WebClient).DownloadString('https://raw.githubusercontent.com/IdentityServer/IdentityServer4.Quickstart.UI/master/getmaster.ps1'))
        //教程https://github.com/IdentityServer/IdentityServer4.Quickstart.UI/tree/master

        //http://localhost:5000/.well-known/openid-configuration

        public IHostingEnvironment Environment { get; }

        public Startup(IHostingEnvironment environment)
        {
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //// for QuickStart-UI
            services.AddMvc();

            var builder = services.AddIdentityServer()
                        .AddInMemoryIdentityResources(Config.GetIdentityResources())
                        .AddInMemoryApiResources(Config.GetApis())
                        .AddInMemoryClients(Config.GetClients())
                        .AddTestUsers(Config.GetUsers());

            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                throw new Exception("need to configure key material");
            }

            //配置允许跨域访问
            services.AddCors(options => options.AddPolicy("AllowAll", p =>
                   p.AllowAnyOrigin()//設置允許來源,Access-Control-Allow-Origin
                   .AllowAnyMethod()//设置允许方法
                   .AllowAnyHeader()//设置允许标头
                   .AllowCredentials()));//Access-Control-Allow-Credentials
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseCors("AllowAll");

            //授权服务
            app.UseIdentityServer();

            // for QuickStart-UI
            // uncomment if you want to support static files
            app.UseStaticFiles();
            // for QuickStart-UI
            // uncomment, if you wan to add an MVC-based UI
            app.UseMvcWithDefaultRoute();

           


        }
    }
}
