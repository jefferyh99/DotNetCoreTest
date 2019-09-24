using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediatRWebTest.PipelineBehavior
{
    /// <summary>
    /// RequestPostProcessorBehavior will execute IRequestPostProcessor implementations after all handlers are called
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    public class MyRequestPostProcessorBehavior<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
    {

        private readonly ILogger _logger;
        public MyRequestPostProcessorBehavior(ILogger<MyRequestPostProcessorBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
        {
            _logger.LogError($"Last1");
            return Task.CompletedTask;
        }
    }
}
