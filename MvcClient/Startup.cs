using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MvcClient.Auths;

namespace MvcClient
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // some details omitted,使jwt变成well-known形式
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            //UI验证Token方式
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies")
                //Implicit
                //.AddOpenIdConnect("oidc", options =>
                //{
                //    options.Authority = "http://localhost:5000";
                //    options.RequireHttpsMetadata = false;

                //    options.ClientId = "mvc";
                //    options.SaveTokens = true;
                //});
                //Hybrid
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";

                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;

                    //mvc
                    //options.ClientId = "mvc";
                    //options.ClientSecret = "secret";//客户端验证
                    //options.ResponseType = "code";//Hybrid，authorization code，identity token

                    options.ClientId = "hybird client";
                    options.ClientSecret = "hybird secret";//客户端验证
                    options.ResponseType = "code id_token";//code=authorization code，id_token=identity token,token=accessToken

                    options.SaveTokens = true;//The OpenID Connect handler saves the tokens (identity, access and refresh in our case) automatically for you.
                    options.GetClaimsFromUserInfoEndpoint = true;

                   
                    options.Scope.Clear();
                    //允许的scope OidcConstants.StandardScopes.OpenId
                    //请求验证服务器，获得相应的scope，如果这边请求的scope，不存在与验证服务器的allowscope列表中时，将会报错invalid scope
                    options.Scope.Add("api1");
                    options.Scope.Add(OidcConstants.StandardScopes.OpenId);
                    options.Scope.Add(OidcConstants.StandardScopes.Profile);
                    //options.Scope.Add(OidcConstants.StandardScopes.Email);
                    options.Scope.Add(OidcConstants.StandardScopes.Phone);
                    //options.Scope.Add(OidcConstants.StandardScopes.Address);
                    options.Scope.Add("roles");
                    options.Scope.Add("locations");

                    options.Scope.Add(OidcConstants.StandardScopes.OfflineAccess);//申请refresh token

                    //集合里的东西，都是要被过滤掉的属性，nbf amr exp...
                    options.ClaimActions.Remove("nbf");
                    options.ClaimActions.Remove("amr");
                    options.ClaimActions.Remove("exp");

                    //拿掉属性 不映射到userclaim中

                    options.ClaimActions.Remove("sid");
                    options.ClaimActions.Remove("sub");
                    options.ClaimActions.Remove("idp");

                    //权限相关
                    //role
                    //让claim里面的角色成为mvc系统识别的角色
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        NameClaimType = JwtClaimTypes.Name,
                        RoleClaimType = JwtClaimTypes.Role
                    };


                });

            //使用策略的方法控制权限
            services.AddAuthorization(option =>
                    {
                        //option.AddPolicy("SmithInSomeWhere", builder =>
                        //{
                        //    builder.RequireAuthenticatedUser();
                        //    builder.RequireClaim(JwtClaimTypes.FamilyName, "Smith", "jack");
                        //    builder.RequireClaim("location", "somewhere");

                        //});

                        option.AddPolicy("SmithInSomewhere", builder =>
                        {
                            builder.AddRequirements(new SmithInSomewhereRequirement());
                        });
                    });

            services.AddSingleton<IAuthorizationHandler, SmithInSomewhereHandler>();
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
                app.UseExceptionHandler("/Home/Error");
            }
            //验证服务
            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();
        }
    }
}
