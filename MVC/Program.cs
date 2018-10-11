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

namespace MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            //var serviceScope = host.Services.CreateScope();

            host.Run();

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

            // 默认配置有很多
            WebHost.CreateDefaultBuilder(args)

                .ConfigureLogging((hostingContext, logging) =>
                {

                    var abc = hostingContext.Configuration.GetSection("Logging");
                    var result = abc.AsEnumerable();
                    logging.ClearProviders();//清除默认配置
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();//不会有效果，因为已经在CreateDefaultBuilder中配置,需要清除默认才有效果
                    logging.AddDebug();
                    var logLevel = LogLevel.Critical;//日志级别

                })
                .UseKestrel()
                .UseStartup<Startup>();




    }
}

