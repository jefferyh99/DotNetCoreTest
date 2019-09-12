using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediatRWebTest.Request
{
    public class Ping : IRequest<string>
    {
    }
}
