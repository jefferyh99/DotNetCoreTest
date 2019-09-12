using MediatR;
using MediatRWebTest.Notification;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediatRWebTest.Handler
{
    public class Pong2Handler : INotificationHandler<PingNotification>
    {
        private readonly ILogger _logger;
        public Pong2Handler(ILogger<Pong2Handler> logger)
        {
            _logger = logger;
        }

        public Task Handle(PingNotification notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Pong 2");
            return Task.CompletedTask;
        }
    }
  
}
