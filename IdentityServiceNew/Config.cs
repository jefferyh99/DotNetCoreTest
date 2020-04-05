using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServiceNew
{
    public static class Config
    {
        /// <summary>
        /// 需要保护的资源（An API is a resource in your system that you want to protect.）
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApis()
        {
            //在scopes出现
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API1 DisplayName",new List<string>{ "location"})
                {
                    ApiSecrets = { new Secret("api1 secret".Sha256())},//当api需要身份验证时，就需要用到这个secret
                },
                new ApiResource("api2", "My API2 DisplayName")
            };
        }

        /// <summary>
        /// a client application that we will use to access our new API.根据这个来颁发token，主要用于配置http请求token时需要配置的数据
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                //主要用于配置登录类型
                new Client
                {
                    ClientId = "client",
                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,//客户端模式，没有用户的概念
                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    //ClientId与ClientSecrets让identity 知道哪个程序正在连接到Identity
                    // scopes that client has access to
                    //允许的范围
                    AllowedScopes = { "api1" },//允许获得权限的API名字范围
                    
                },

                // resource owner password grant client
                new Client
                {
                    ClientId = "ro.client",//资源拥有者模式
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,//需要传递用户名密码，所有用户名，密码都允许

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1" }
                },

                 // OpenID Connect implicit flow client (MVC)
                new Client
                {
                    ClientId = "mvcold",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Implicit,//简易模式,//The implicit grant type is optimized for browser-based applications.MVC类型

                    // where to redirect to after login
                    RedirectUris = { "http://localhost:5002/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },


                    AlwaysIncludeUserClaimsInIdToken = false,

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,//subject id
                        IdentityServerConstants.StandardScopes.Profile//first name, last name etc..对应的是GetUsers里面的用户数据
                    }
                },
                //mvc
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                     //推荐做法
                    //AllowedGrantTypes = GrantTypes.Hybrid,//Hybrid flow is a combination of the implicit and authorization code flow（It is used for server-side web applications and native desktop/mobile applications.）
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    RedirectUris = { "http://localhost:5002/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },
                    FrontChannelLogoutUri = "http://localhost:5002/signout-oidc",

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1", //允许获得权限的API名字范围,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Phone,



                    },
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 60,//60seconds
                    
                },
                //https://identityserver4.readthedocs.io/en/latest/topics/grant_types.html


                // JavaScript Client
                new Client
                {
                    ClientId = "js",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Code,//Authorization code，授权码模式，内部实现了授权码，直接返回了AccessToken
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris =           { "http://localhost:5003/callback.html" },
                    PostLogoutRedirectUris = { "http://localhost:5003/index.html" },
                    AllowedCorsOrigins =     { "http://localhost:5003" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    }
                },

                 //angular
                new Client
                {
                    ClientId = "angular-client",
                    ClientName = "angular spa 客戶端",
                    ClientUri = "http://localhost:4200",

                    AllowedGrantTypes = GrantTypes.Implicit,//Authorization code，授权码模式，内部实现了授权码，直接返回了AccessToken
                    RequireConsent = true,
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 60*5,

                    //登錄成功後回調
                    RedirectUris =           { "http://localhost:4200/signin-oidc","http://localhost:4200/redirect-silentrenew" },//後面這個用於自動刷新
                    PostLogoutRedirectUris = { "http://localhost:4200" },
                    AllowedCorsOrigins =     { "http://localhost:4200" },

                    AllowedScopes =
                    {
                         IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1", //允许获得权限的API名字范围,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Phone,
                    }
                },

                //hybird
                new Client()
                {
                    ClientId = "hybird client",
                    ClientName = "ASP.NET Core Hybird 客户端",
                    ClientSecrets = {new Secret("hybird secret".Sha256())},
                    AllowedGrantTypes = new []{GrantType.Hybrid },
                    RedirectUris = { "http://localhost:5002/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },
                    FrontChannelLogoutUri = "http://localhost:5002/signout-oidc",
                    AllowOfflineAccess=true,//是否允许申请refresh token，
                    AccessTokenType = AccessTokenType.Reference,//jwt不会每次都验证token，里面有包含过期等信息，自适应的。reference的话每次都需要调用ldp服务去验证，会频繁验证
                    AlwaysIncludeUserClaimsInIdToken=true,
                    //允许客户端请求获得的所有权限
                    AllowedScopes=
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1", //允许获得权限的API名字范围,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Phone,
                        "roles",
                        "locations",
                    },

                }
            };
        }

        /// <summary>
        /// 不推荐通过账号密码获取到token，但是这个可用于登录login UI与获取个人资料信息
        /// </summary>
        /// <returns></returns>
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password",

                    //User profile
                    Claims = new []
                    {
                        new Claim("name", "Alice"),
                        new Claim("website", "https://alice.com")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",//会在payload的sub中显示
                    Username = "bob",
                    Password = "password",

                    //User profile
                    Claims = new []
                    {
                        new Claim("name", "Bob"),
                        new Claim("website", "https://bob.com"),
                        new Claim("location","somewhere"),
                        new Claim(JwtClaimTypes.FamilyName,"Smith"),
                        new Claim(JwtClaimTypes.GivenName,"Bob"),
                    }
                }
            };
        }
        //https://github.com/solenovex/Identity-Server-4-Tutorial-Demo-Code/blob/master/07-08.%20Hybrid%20Flow%20-%20MVC%20Client/Idp/Config.cs
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),//subject id
                new IdentityResources.Profile(),//first name, last name etc..
                new IdentityResources.Address(),
                new IdentityResources.Phone(),
                new IdentityResources.Email(),
                new IdentityResource("roles","角色",new List<string>{JwtClaimTypes.Role }),
                new IdentityResource("locations","地点",new List<string>{"location"}),//前面是scope，后面是claimType
                //allowScpoe以一个IdentityResource为范围，一个IdentityResource能包含多个后面是claimType，用户携带的是claimType
            };


        }

    }
}
