using AspectCore.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoFactTest.Interceptor
{
    /// <summary>
    /// 面向切面 Aop AspectCore
    /// </summary>
    public class CustomInterceptorAttribute : AbstractInterceptorAttribute
    {
        private readonly string _name;
        public CustomInterceptorAttribute(string name)
        {
            _name = name;
        }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                Console.WriteLine("Before service call."+ _name);
                await next(context);
            }
            catch (Exception)
            {
                Console.WriteLine("Service threw an exception!" + _name);
                throw;
            }
            finally
            {
                Console.WriteLine("After service call." + _name);
            }
        }
    }

}
