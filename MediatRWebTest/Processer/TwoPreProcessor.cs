using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediatRWebTest.Processer
{
    public class TwoPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger _logger;
        public TwoPreProcessor(ILogger<TwoPreProcessor<TRequest>> logger)
        {
            _logger = logger;
        }
        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            _logger.LogError($"TwoPreProcessor before");
            return Task.CompletedTask;
        }
    }
}
