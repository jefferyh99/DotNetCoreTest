using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;

namespace IdentityServiceEF
{
    public class Startup
    {
        //添加默认QuickStart-UI
        //iex ((New-Object System.Net.WebClient).DownloadString('https://raw.githubusercontent.com/IdentityServer/IdentityServer4.Quickstart.UI/master/getmaster.ps1'))
        //教程https://github.com/IdentityServer/IdentityServer4.Quickstart.UI/tree/master

        //http://localhost:5000/.well-known/openid-configuration

        public IHostingEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            //结果IdentityServiceEF
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;


            //var builder = services.AddIdentityServer()
            //            .AddTestUsers(Config.GetUsers());
            //            .AddInMemoryIdentityResources(Config.GetIdentityResources())
            //            .AddInMemoryApiResources(Config.GetApis())
            //            .AddInMemoryClients(Config.GetClients())



            // configure identity server with in-memory stores, keys, clients and scopes
            var builder = services.AddIdentityServer()
                .AddTestUsers(Config.GetUsers())
                // this adds the config data from DB (clients, resources)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                // the call to MigrationsAssembly is used to inform EF the host project that will contain the migrations code 
                // (which is necessary since it is a different than the assembly that contains the DbContext classes).
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;

                });

            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                throw new Exception("need to configure key material");
            }

            //// for QuickStart-UI
            services.AddMvc();

            //配置允许跨域访问
            services.AddCors(options => options.AddPolicy("AllowAll", p =>
                   p.AllowAnyOrigin()//設置允許來源,Access-Control-Allow-Origin
                   .AllowAnyMethod()//设置允许方法
                   .AllowAnyHeader()//设置允许标头
                   .AllowCredentials()));//Access-Control-Allow-Credentials
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // this will do the initial DB population
            InitializeDatabase(app);

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseCors("AllowAll");

            //授权服务
            app.UseIdentityServer();

            // for QuickStart-UI
            // uncomment if you want to support static files
            app.UseStaticFiles();
            // for QuickStart-UI
            // uncomment, if you wan to add an MVC-based UI
            app.UseMvcWithDefaultRoute();




        }

        //根据Migration结构，生成数据库结构。
        //Migration
        //dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb
        //dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb
        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                #region 数据库结构迁移
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();

                #endregion

                #region 数据迁移（这边没有TestUser）

                if (!context.Clients.Any())
                {
                    foreach (var client in Config.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Config.GetApis())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
                #endregion
            }
        }
    }
}
