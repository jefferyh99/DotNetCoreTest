using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MediatR;
using MediatR.Pipeline;
using MediatRWebTest.PipelineBehavior;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MediatRWebTest
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //加载当前assembly中的所有数据
            //https://github.com/jbogard/MediatR.Extensions.Microsoft.DependencyInjection
            //Scans assemblies and adds handlers, preprocessors, and postprocessors implementations to the container
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);

            //一样的效果
            //var a = typeof(Startup).GetTypeInfo().Assembly;
            //var b = typeof(Startup).Assembly;


            //依赖注入,按注册顺序来执行管道（每一次请求），这边的注册需要手动注册，因为需要确定顺序，这边的执行顺序为：
            
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingTwoBehavior<,>));

            //说明：如果是继承多个IRequestPostProcessor与IRequestPostProcessor，他们的注册顺序不能指定，mediatr会默认执行。
            //只会坚持在请求执行前执行，与请求执行后执行。

            //如果需要有特定顺序的话，继承IPipelineBehavior，并按顺序注册即可。
            //只有IRequestHandler<TRequest,TResponse>才能使用Behavior，INotificationHandler<TRequest>不能使用


            //Pre-LoggingBehavior-LoggingTwoBehavior-Post

            //如果使用MediatR.Extensions.Microsoft.DependencyInjection包时，会自动注册，不需要再手动注册了。
            //services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            //services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));

            //services.AddScoped(typeof(IRequestPreProcessor<>), typeof(MyRequestPreProcessorBehavior<>));
            //services.AddScoped(typeof(IRequestPostProcessor<,>), typeof(MyRequestPostProcessorBehavior<,>));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
