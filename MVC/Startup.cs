using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System.Text.Encodings;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.Extensions.WebEncoders;
using System.Globalization;

namespace MVC
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

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            #region 可能解决中文被编码
            //解决中文被编码
            //services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));

            //services.Configure<WebEncoderOptions>(options =>
            //{
            //    options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            //});



            //services.Configure<WebEncoderOptions>(options =>options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.BasicLatin,UnicodeRanges.CjkUnifiedIdeographs));
            #endregion

            ////添加定时器
            //services.AddHostedService<TimedHostedService>();

            services.AddMvc(option => { }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            #region 依赖注入
            //services.AddScoped<IMyDependency, MyDependency>();
            //services.AddTransient<IOperationTransient, Operation>();
            //services.AddScoped<IOperationScoped, Operation>();
            //services.AddSingleton<IOperationSingleton, Operation>();
            //services.AddSingleton<IOperationSingletonInstance>(new Operation(Guid.Empty));

            // OperationService depends on each of the other Operation types.
            //services.AddTransient<OperationService, OperationService>();
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            //env.EnvironmentName = EnvironmentName.Production;
            //注意加载顺序
            var environmentName = EnvironmentName.Production;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //自定义状态页
                // Expose the members of the 'Microsoft.AspNetCore.Http' namespace 
                // at the top of the file:
                // using Microsoft.AspNetCore.Http;
                //app.UseStatusCodePages(async context =>
                //{
                //    context.HttpContext.Response.ContentType = "text/plain";

                //    await context.HttpContext.Response.WriteAsync(
                //        "Status code page, status code: " +
                //        context.HttpContext.Response.StatusCode);
                //});
            }
            else
            {
                //异常处理中间件，还有很多可选选项
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            #region app.Use
            //app.Use((context, next) =>
            //{
            //    context.Response.WriteAsync("start 1 我是中文\r\n");
            //    //调用管道中的下一个委托
            //    context.Response.WriteAsync("end 1 我是中文\r\n");
            //    return next.Invoke();

            //});


            ////Use方法，则是在管道中增加一个Middleware。如果调用了next.Invoke()方法，它会去执行下一个Middleware 
            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync("start 3\r\n");
            //    //调用管道中的下一个委托
            //    await next.Invoke();
            //    await context.Response.WriteAsync("end 3\r\n");

            //    //调用管道中的下一个委托
            //    //使用Use方法，而没有调用next.Invoke()，Use的效果与Run的效果是一致的。
            //    //await next.Invoke();
            //});

            ////符合路由规则才执行,也会按顺序执行
            //app.Map("/mapTest", HandleMap);

            ////符合规则才执行
            //app.MapWhen(context =>
            //{
            //    return context.Request.Query.ContainsKey("q");
            //}, HandleMap);

            ////Run方法在说明上是这样的:在管道的尾端增加一个Middleware；它是执行的最后一个Middleware。即它执行完就不再执行下一个Middleware了,不一定要有
            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync("start 2\r\n");
            //    await context.Response.WriteAsync("Hello from 2nd delegate.\r\n");
            //    await context.Response.WriteAsync("end 2\r\n");
            //});



            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync("start 4\r\n");
            //    await context.Response.WriteAsync("Hello from 4th delegate.\r\n");
            //    await context.Response.WriteAsync("end 4\r\n");
            //});
            #endregion

        }

        private static void HandleMap(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello ,that is Handle Map ");
            });
        }
    }

}
