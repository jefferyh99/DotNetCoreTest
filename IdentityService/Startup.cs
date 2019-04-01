using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            InMemoryConfiguration.Configuration = this.Configuration;

            services.AddIdentityServer()
            .AddDeveloperSigningCredential()
            //生产环境时需要使用AddSigningCredential()
            .AddTestUsers(InMemoryConfiguration.GetUsers().ToList())
            .AddInMemoryClients(InMemoryConfiguration.GetClients())
            .AddInMemoryApiResources(InMemoryConfiguration.GetApiResources())
            .AddInMemoryIdentityResources(InMemoryConfiguration.GetIdentityResources());


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
        }
    }

    /// <summary>
    /// （1）哪些API可以使用这个AuthorizationServer
    　　/// （2）哪些Client可以使用这个AuthorizationServer
    　　/// （3）哪些User可以被这个AuthrizationServer识别并授权
    /// 返回的scope与Claim
    /// </summary>
    public class InMemoryConfiguration
    {
        public static IConfiguration Configuration { get; set; }
        /// <summary>
        /// Define which APIs will use this IdentityServer(AllowedScopes)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                //用于绑定站点的,微服务的站点名称
                new ApiResource("clientservice", "CAS Client Service"){
                   ApiSecrets = { new Secret("api1pwd".Sha256()) },
                   Scopes = new List<Scope>(){
                       new Scope()
                       {
                           Name = "clientservice.Scope1",
                           UserClaims = new List<string>(){//用户,TestUser下面的，且传入的类型必须一致
                               ClaimTypes.Role,
                               ClaimTypes.Name
                           }
                       }
                   },
                   UserClaims = new List<string>(){
                       ClaimTypes.Role
                   },
                   

                },
                new ApiResource("productservice", "CAS Product Service"){
                     ApiSecrets = { new Secret("api1pwd".Sha256()) },
                },
                new ApiResource("agentservice", "CAS Agent Service")
            };
        }

        /// <summary>
        /// Define which Apps will use thie IdentityServer,用于获取AccessToken
        /// grant_type = client_credentials
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client
                {
                    ClientId = "client.api.service",//client_id
                    ClientSecrets = new [] { new Secret("clientsecret".Sha256())},//client_secret
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,//grant_type
                    AllowedScopes = new [] { "clientservice","clientservice.Scope1" },//某站点可以通过这个服务授权
                    AccessTokenType = AccessTokenType.Jwt,
                    Claims = new List<Claim>(){
                        //new Claim(ClaimTypes.Role,"superman"),
                        //new Claim(ClaimTypes.Name,"Name"),
                        //new Claim(ClaimTypes.MobilePhone,"123456789")
                    },
                    

                },
                new Client
                {
                    ClientId = "product.api.service",
                    ClientSecrets = new [] { new Secret("productsecret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = new [] { "clientservice", "productservice","clientservice.Scope1" },//如果ApiResource有子scope时，这边需要配置子scope


                },
                new Client
                {
                    ClientId = "agent.api.service",
                    ClientSecrets = new [] { new Secret("agentsecret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = new [] { "agentservice", "clientservice", "productservice" },
                }
            };
        }

        /// <summary>
        /// Define which uses will use this IdentityServer,用于获取AccessToken,
        /// ResourceOwner
        /// grant_type = password
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TestUser> GetUsers()
        {
            return new[]
            {
                new TestUser
                {
                    SubjectId = "10001",
                    Username = "edison@hotmail.com",
                    Password = "edisonpassword",
                    Claims = new List<Claim>(){
                        new Claim(ClaimTypes.Role,"superman"),
                        new Claim(ClaimTypes.MobilePhone,"1234567891")
                    },                  
                },
                new TestUser
                {
                    SubjectId = "10002",
                    Username = "andy@hotmail.com",
                    Password = "andypassword"
                },
                new TestUser
                {
                    SubjectId = "10003",//sub 主题
                    Username = "leo@hotmail.com",
                    Password = "leopassword"
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId()
            };
        }
    }


}
