using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspectCore.Configuration;
using AspectCore.Extensions.Autofac;
//using AspectCore.Extensions.DependencyInjection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoFactTest.Dependency;
using AutoFactTest.Interceptor;
using AutoFactTest.Model;
using AutoFactTest.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AutoFactTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //要使用第三方容器，Startup.ConfigureServices 必须返回 IServiceProvider。
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //Option注册
            services.Configure<ContentList>(Configuration.GetSection("ContentList"));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            #region Add Autofac 直接使用(注意使用注入时需要返回IServiceProvider，默认的是void方式)
            // Add Autofac
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<DefaultModule>();

            #region Aspect


            containerBuilder.RegisterDynamicProxy(config =>
                {
                    //过滤器
                    config.Interceptors.AddTyped<CustomInterceptorAttribute>(args: new object[] { "custom" }
                    //, predicates: method => method.DeclaringType.Name.EndsWith("Service"));//被定位到的服务
                    // , predicates: Predicates.ForService("*Service")//通配符定位
                    );
                    config.Interceptors.AddTyped<MethodExecuteLoggerInterceptor>();

                    

                    #region 不被代理
                    //最后一级为App1的命名空间下的Service不会被代理
                    config.NonAspectPredicates.AddNamespace("*.App1");
                    //ICustomService接口不会被代理
                    config.NonAspectPredicates.AddService("ICustomService");

                    //后缀为Service的接口和类不会被代理
                    config.NonAspectPredicates.AddService("*Service");

                    //命名为Query的方法不会被代理
                    config.NonAspectPredicates.AddMethod("Query");

                    //后缀为Query的方法不会被代理
                    config.NonAspectPredicates.AddMethod("*Query");
                    #endregion

                });

            #endregion
            containerBuilder.Populate(services);
            var container = containerBuilder.Build();

            return new AutofacServiceProvider(container);
            #endregion

            #region Add Autofac With Interface 接口使用(注意使用注入时需要返回IServiceProvider，默认的是void方式)

            //var autoFactDependencyResolver = new AutoFacDependencyResolver(services);
            //IoC.Initialize(autoFactDependencyResolver);//反模式
            //return new AutofacServiceProvider(autoFactDependencyResolver.Container);

            #endregion

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
