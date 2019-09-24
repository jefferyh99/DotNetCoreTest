using MediatR;
using MediatRWebTest.Request;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediatRWebTest.Handler
{
    //IRequest<T> - the request returns a value
    //IRequest - the request does not return a value

    //IRequestHandler<T, U> - implement this and return Task<U>
    //RequestHandler<T, U> - inherit this and return U

    //IRequestHandler<T> - implement this and you will return Task<Unit>.
    //AsyncRequestHandler<T> - inherit this and you will return Task.
    //RequestHandler<T> - inherit this and you will return nothing (void).


    public class PingHandler : IRequestHandler<Ping, string>
    {
        private readonly ILogger _logger;
        public PingHandler(ILogger<PingHandler> logger)
        {
            _logger = logger;
        }

        public Task<string> Handle(Ping request, CancellationToken cancellationToken)
        {
            _logger.LogError($"PingHandler");
            return Task.FromResult("Pong");
        }
    }
}
