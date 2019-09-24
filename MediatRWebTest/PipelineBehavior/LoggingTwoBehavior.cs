using MediatR;
using MediatRWebTest.Controllers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediatRWebTest.PipelineBehavior
{
    public class LoggingTwoBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _logger;
        public LoggingTwoBehavior(ILogger<LoggingTwoBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogError($"Handling2 {typeof(TRequest).Name}");
            var response = await next();
            _logger.LogError($"Handled2 {typeof(TResponse).Name}");
            return response;
        }
    }
}
