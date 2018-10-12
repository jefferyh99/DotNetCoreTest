using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {

            // NLog: setup the logger first to catch all errors
            //var logger = NLog.Web.NLogBuilder.ConfigureNLog("Config/nlog.config").GetCurrentClassLogger();
            //编译时会有缓存问题，坑爹啊
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("Config/nlogTracking.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");

                var host = CreateWebHostBuilder(args).Build();

                //var serviceScope = host.Services.CreateScope();

                host.Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }




            //通用主机，非web资源，如后台定时器等等
            //new HostBuilder().ConfigureHostConfiguration(configHost =>
            //{
            //    configHost.SetBasePath(Directory.GetCurrentDirectory());
            //    configHost.AddJsonFile("hostsettings.json", optional: true);
            //    configHost.AddEnvironmentVariables(prefix: "PREFIX_");
            //    configHost.AddCommandLine(args);
            //}).Build().Run();

        }

        //web 主机
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>

            // 默认配置有很多,默认是有appsettings.Development.json配置的
            //config.AddJsonFile("appsettings.json", true, true).AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", true, true);
            WebHost.CreateDefaultBuilder(args)

            //自带默认log配置
            //.ConfigureLogging((hostingContext, logging) =>
            //{
            //    var abc = hostingContext.Configuration.GetSection("Logging");
            //    var result = abc.AsEnumerable();
            //    logging.ClearProviders();//清除默认配置
            //    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            //    logging.AddConsole();//不会有效果，因为已经在CreateDefaultBuilder中配置,需要清除默认才有效果
            //    logging.AddDebug();
            //    var logLevel = LogLevel.Critical;//日志级别
            //})
            .UseKestrel()
            .UseStartup<Startup>()
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            })
            .UseNLog();  // NLog: setup NLog for Dependency injection
       

    }
}

