using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SampleApp
{
    public class Program
    {
        private static readonly Dictionary<string, Type> _scenarios;

        static Program()
        {
            _scenarios = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
            {
                { "basic", typeof(BasicStartup) },
                { "db", typeof(DbHealthStartup) },
                { "dbcontext", typeof(DbContextHealthStartup) },
                { "liveness", typeof(LivenessProbeStartup) },
                { "writer", typeof(CustomWriterStartup) },
                { "port", typeof(ManagementPortStartup) },

            };
        }
        //官方文档：https://docs.microsoft.com/zh-cn/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-3.0
        //事例代码：https://github.com/aspnet/AspNetCore.Docs/tree/master/aspnetcore/host-and-deploy/health-checks/samples/3.x/HealthChecksSample
        public static void Main(string[] args)
        {
            BuildHost(args).Run();
        }

        public static IHost BuildHost(string[] args)
        {
            var config1 = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables(prefix: "ASPNETCORE_")
                .AddCommandLine(args)
                .Build();

            //服务器CommandLine输入配置
            var scenario = config1["scenario"] ?? "basic";

            if (!_scenarios.TryGetValue(scenario, out var startupType))
            {
                startupType = typeof(BasicStartup);
            }

            return new HostBuilder()
                .ConfigureAppConfiguration(config =>
                {
                    config.AddConfiguration(config1);
                })
                .ConfigureLogging(builder =>
                {
                    builder.SetMinimumLevel(LogLevel.Trace);
                    builder.AddConfiguration(config1);
                    builder.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel();
                    webBuilder.UseStartup(startupType);
                })
                .Build();
        }
    }
}
