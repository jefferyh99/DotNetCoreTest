using AutoFactTest.Interceptor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoFactTest
{
    public interface ITest
    {
        //[NonAspect]//不被代理
        //[CustomInterceptor]
        string GetTestName();
    }
}
