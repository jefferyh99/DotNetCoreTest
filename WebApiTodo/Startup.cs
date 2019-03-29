using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using TodoApi.Models;

namespace WebApiTodo
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
            services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //Install-Package IdentityServer4.AccessTokenValidation
            //IdentityServer
            services.AddMvcCore().AddAuthorization().AddJsonFormatters();
            services.AddAuthentication(Configuration["Identity:Scheme"]).AddIdentityServerAuthentication(options => {
                options.RequireHttpsMetadata = false;//// for dev env
                options.Authority = $"http://{Configuration["Identity:IP"]}:{Configuration["Identity:Port"]}";
                options.ApiName = Configuration["Service:Name"]; // match with configuration in IdentityServer
                options.ApiSecret = Configuration["Service:Password"];
                

            });




            //Install-Package Swashbuckle.AspNetCore
            // Register the Swagger generator, defining 1 or more Swagger documents
            // 头信息
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {
                    Title = "My API",
                    Version = "v1",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = "None",//服务条款,
                    Contact = new Contact
                    {
                        Name = "Shayne Boyer",
                        Email = string.Empty,
                        Url = "https://twitter.com/spboyer"
                    },
                    License = new License
                    {
                        Name = "Use under LICX",
                        Url = "https://example.com/license"
                    }
                });

                //添加summary注释
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            app.UseHttpsRedirection();

            //app.UseStaticFiles();

            // authentication
            app.UseAuthentication();

            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                //http://localhost:<port>/swagger/v1/swagger.json
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");//如果使用目录及 IIS 或反向代理，请使用 ./ 前缀将 Swagger 终结点设置为相对路径。
                //c.RoutePrefix = "";//http://localhost:<port>/index.html)或者修改launchSettings.json
            });
        }
    }
}
