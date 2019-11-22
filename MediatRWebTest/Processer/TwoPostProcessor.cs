using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediatRWebTest.Processer
{
    public class TwoPostProcessor<TRequest,TResponse> : IRequestPostProcessor<TRequest,TResponse>
    {
        private readonly ILogger _logger;
        public TwoPostProcessor(ILogger<TwoPostProcessor<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }


        public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
        {
            _logger.LogError($"TwoPostProcessor after");
            return Task.CompletedTask;
        }
    }
}
