using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Greet.V1;
using Grpc.Net.Client;

namespace GrpcGreeterClient
{
    class Program
    {
        /// <summary>
        /// https://docs.microsoft.com/zh-cn/aspnet/core/tutorials/grpc/grpc-start?view=aspnetcore-3.1&tabs=visual-studio
        /// Install-Package Grpc.Net.Client
        /// Install-Package Google.Protobuf
        /// Install-Package Grpc.Tools
        /// </summary>
        /// <param name="args"></param>
        static async Task Main(string[] args)
        {
            Greeter();

            await GreeterRequestStreamAsync();
            await GreeterResonseStreamAsync();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        public static void Greeter()
        {
            //通道，成本高昂，建议重用（工厂集成：https://docs.microsoft.com/zh-cn/aspnet/core/grpc/clientfactory?view=aspnetcore-3.1）
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");

            //客户端，成本低，无需重用
            var client = new Greeter.GreeterClient(channel);
            var reply = client.SayHello(new HelloRequest() { Name = "GreeterClient" });
            Console.WriteLine("Greeting: " + reply.Message);

        }

        public static async Task GreeterResonseStreamAsync()
        {
            // The port number(5001) must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");

            //var client = new GrpcGreeter.Greeter.GreeterClient(channel);
            //var reply = await client.SayHelloAsync(new HelloRequest { Name = "GreeterClient" });


            var client = new GreeterResponseStream.GreeterResponseStreamClient(channel);
            var reply = client.SayHello(new HelloRequest() { Name = "GreeterResponseStreamClient" });



            CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var token = cts.Token;


            //var token = new CancellationToken(false);

            try
            {
                int flag = 0;
                while (await reply.ResponseStream.MoveNext(token))
                {
                    Console.WriteLine("Greeting: " + reply.ResponseStream.Current.Message);
                    flag++;
                    if (flag == 20)
                    {
                        cts.Cancel();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is Grpc.Core.RpcException)
                {
                    if (((Grpc.Core.RpcException)ex).StatusCode == Grpc.Core.StatusCode.Cancelled)
                    {
                        cts.Dispose();
                        Console.WriteLine("用户已取消");
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

        }

        public static async Task GreeterRequestStreamAsync()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");

            //var client = new GrpcGreeter.Greeter.GreeterClient(channel);
            //var reply = await client.SayHelloAsync(new HelloRequest { Name = "GreeterClient" });

            var client = new GreeterRequestStream.GreeterRequestStreamClient(channel);
            using var call = client.SayHello();

            for (int i = 1; i <= 10; i++)
            {
                await call.RequestStream.WriteAsync(new HelloRequest { Name = "GreeterRequestStreamClient at" + i });
            }

            await call.RequestStream.CompleteAsync();

            var response = await call;

            Console.WriteLine($"Message: {response.Message}");

            
        }
    }
}
