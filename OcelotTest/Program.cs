using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ocelot.Middleware;
using Ocelot.DependencyInjection;

namespace OcelotTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = CreateWebHostBuilder(args);
            var build = builder.Build();
            build.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>

        //WebHost.CreateDefaultBuilder(args)
        // .ConfigureAppConfiguration((hostingContext, config) =>
        // {
        //     config

        //         .AddJsonFile("Config/ocelot.json")
        //         .AddEnvironmentVariables();
        // })
        //    .UseStartup<Startup>();

        new WebHostBuilder()
               .UseKestrel()
               .UseContentRoot(Directory.GetCurrentDirectory())
               .ConfigureAppConfiguration((hostingContext, config) =>
               {
                   config
                       .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                       .AddJsonFile("appsettings.json", true, true)
                       .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                       //.AddJsonFile("Config/ocelot.json")//单一配置
                       .AddOcelot("OcelotConfig", hostingContext.HostingEnvironment)//合并配置,匹配(?i)ocelot.([a-zA-Z0-9]*).json 
                       .AddEnvironmentVariables();
               })
                 .ConfigureServices(s =>
                 {
                     s.AddOcelot();
                 })
               .ConfigureLogging((hostingContext, logging) =>
               {
                   logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                   logging.AddConsole();
                   logging.AddDebug();
                   logging.AddEventSourceLogger();
               })
               .UseIISIntegration()
               .Configure(app =>
               {
                   app.UseOcelot().Wait();
               });
        //.UseIIS()//**Note:** When using ASP.NET Core 2.2 and you want to use In-Process hosting, replace **.UseIISIntegration()** with **.UseIIS()**, otherwise you'll get startup errors.
        //.UseStartup<Startup>();
    }
}
