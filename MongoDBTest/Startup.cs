using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDBTest.Repository;
using MongoDBTest.Repository.Base;
using MongoDBTest.Setting;

namespace MongoDBTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // 通过注入服务的方式，让程序获得某些能力
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //配置注入
            services.Configure<MongoDBSettings>(Configuration.GetSection("MongoConnection"));
            services.AddTransient<IBookRepository, BookRepository>();

            //配置绑定，在当前配置中使用
            //var list = new List<RabbitMQSetting>();
            //Configuration.GetSection("TrackingCrawlerRabbitMQSettings").Bind(list);
            //for (int i = 0; i < list.Count; i++)
            //{
            //    services.Configure<RabbitMQSetting>(list[i].ServerName, Configuration.GetSection($"TrackingCrawlerRabbitMQSettings:{i}"));
            //}
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
