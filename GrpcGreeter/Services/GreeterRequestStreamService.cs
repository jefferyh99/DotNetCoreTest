using Greet.V1;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcGreeter
{
    public class GreeterRequestStreamService : GreeterRequestStream.GreeterRequestStreamBase
    {
        public override async Task<HelloReply> SayHello(IAsyncStreamReader<HelloRequest> requestStream, ServerCallContext context)
        {
            var messages = new List<string>();

            while (await requestStream.MoveNext())
            {
                var message = requestStream.Current;
                messages.Add(message.Name);
            }
            return new HelloReply()
            {
                Message = string.Join(",", messages)
        };
    }
}
}
