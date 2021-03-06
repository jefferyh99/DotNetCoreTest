﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Mongo;
using HangFireTest.Job;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HangFireTest
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var connectionString = Configuration.GetSection("HangfireMongoConnection:ConnectionString").Value;
            var dataBaseName = Configuration.GetSection("HangfireMongoConnection:Database").Value;

            services.AddHangfire(config =>
            {
                config.UseMongoStorage(connectionString, dataBaseName, new MongoStorageOptions()
                {
                    Prefix = "HF",
                    MigrationOptions = new MongoMigrationOptions()
                    {
                        Strategy = MongoMigrationStrategy.Migrate,
                        BackupStrategy = MongoBackupStrategy.Collections
                    }
                });

                //沙箱默认log
                config.UseColouredConsoleLogProvider();

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
                app.UseHsts();
            }

            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                Authorization = new[] { new CustomAuthorizeFilter() }
            });
            app.UseHangfireServer(new BackgroundJobServerOptions()
            {
                ServerName = "ABC" + Guid.NewGuid().ToString()
            });

            HangfireJobManager.ConfigJob();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
