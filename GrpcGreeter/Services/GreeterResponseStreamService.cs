using Greet.V1;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcGreeter
{
    public class GreeterResponseStreamService : GreeterResponseStream.GreeterResponseStreamBase
    {
        public override async Task SayHello(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            while (!context.CancellationToken.IsCancellationRequested)
            {
                await responseStream.WriteAsync(new HelloReply() { Message = "Hello " + request.Name + " at " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }
    }
}
