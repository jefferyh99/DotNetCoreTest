﻿using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediatRWebTest.PipelineBehavior
{
    /// <summary>
    /// RequestPreProcessorBehavior will execute IRequestPreProcessor implementations before any handlers are called
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class MyRequestPreProcessorTwoBehavior<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger _logger;
        public MyRequestPreProcessorTwoBehavior(ILogger<MyRequestPreProcessorTwoBehavior<TRequest>> logger)
        {
            _logger = logger;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            _logger.LogError($"Before2");
            return Task.CompletedTask;
        }
    }
}
