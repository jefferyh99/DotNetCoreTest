using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Extensions.Microsoft.DependencyInjection;

namespace MediatRTest
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection.AddMediatR(Assembly)

            var response = await mediator.Send(new Ping());
            Console.WriteLine(response); // "Pong"

            Console.ReadKey();
        }
    }

    public class Ping : IRequest<string> { }

    public class PingHandler : IRequestHandler<Ping, string>
    {
        public Task<string> Handle(Ping request, CancellationToken cancellationToken)
        {
            return Task.FromResult("Pong");
        }
    }
}
