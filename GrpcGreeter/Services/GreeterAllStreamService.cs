using Greet.V1;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcGreeter
{
    public class GreeterAllStreamService : GreeterAllStream.GreeterAllStreamBase
    {
        public override async Task SayHello(IAsyncStreamReader<HelloRequest> requestStream, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            var messages = new List<string>();
            // Read requests in a background task.
            var readTask = Task.Run(async () =>
            {
                await foreach (var message in requestStream.ReadAllAsync())
                {
                    // Process request.
                    var message1 = requestStream.Current;
                    messages.Add(message1.Name);
                }
            });

            // Send responses until the client signals that it is complete.
            while (!readTask.IsCompleted)
            {
                await responseStream.WriteAsync(new HelloReply() { Message = "Hello " + string.Join(",", messages) + " at " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                await Task.Delay(TimeSpan.FromSeconds(1), context.CancellationToken);
            }
        }
    }
}
