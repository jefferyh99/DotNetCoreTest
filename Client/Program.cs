// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        private static async Task Main()
        {
            // discover endpoints from metadata
            var client = new HttpClient();

            //http://localhost:5000/.well-known/openid-configuration
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                Console.ReadKey();
                return;
            }

            await ResourceOwnerPassword(disco);


        }

        private static async Task ClientCredentials(DiscoveryResponse disco)
        {
            var client = new HttpClient();
            // request token
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,//http://localhost:5000/connect/token
                ClientId = "client",//客户名字
                ClientSecret = "secret",//密码
                Scope = "api1"//需要访问的API，或者说需要获得的API访问权限，需要当前客户下有对应的Scope才会赋予权限
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                Console.ReadKey();
                return;
            }
            //获得JWT token
            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            // 调用API接口
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);
            //API站点需要与颁发token的站点沟通，以确认token的有效性
            var response = await apiClient.GetAsync("http://localhost:5001/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }

            Console.ReadKey();
        }

        private static async Task ResourceOwnerPassword(DiscoveryResponse disco)
        {
            var client = new HttpClient();
            // request token
            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "ro.client",
                ClientSecret = "secret",
                Scope = "api1",

                UserName = "alice",
                Password = "password",
                //UserName = "bob",
                //Password = "password",

            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.ReadKey();
        }
    }
}