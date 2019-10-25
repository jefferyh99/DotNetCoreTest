using AspectCore.DynamicProxy;
using AspectCore.Injector;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoFactTest.Interceptor
{
    /// <summary>
    /// 1调用方法：[EventHandlerRetryInterceptor(1, false, false)]
    /// 2必须注册：containerBuilder.RegisterDynamicProxy(）
    /// </summary>
    public class EventHandlerRetryInterceptorAttribute : AbstractInterceptorAttribute
    {
        private readonly int _retryCount;
        private readonly bool _rollbackBeforeRetry;
        private readonly bool _throwException;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="retryCount">重试次数</param>
        /// <param name="rollbackBeforeRetry">在重试前是否回滚之前的实物</param>
        /// <param name="throwException">是否需要抛异常</param>
        public EventHandlerRetryInterceptorAttribute(int retryCount, bool rollbackBeforeRetry, bool throwException)
        {
            _retryCount = retryCount;
            _rollbackBeforeRetry = rollbackBeforeRetry;
            _throwException = throwException;
        }

        [FromContainer]
        public ILogger<EventHandlerRetryInterceptorAttribute> Logger { get; set; }

        [FromContainer]
        public IDBContext DbContext { get; set; }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            //try
            //{
            //    var policy = Policy.Handle<System.Exception>()
            //   .WaitAndRetryAsync(_retryCount,
            //       retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            //       (ex, time, count, ctx) =>
            //       {
            //           Logger.LogWarning(ex, $"Handle Event [{context.GetType()}] Error. The Error Message is {ex.ToString()}, retryCount:{_retryCount}，count:{count}");
            //           if (_rollbackBeforeRetry)
            //           {
            //               DbContext.RollbackAsync().Wait(); //异常后回滚事务.再重试
            //           }
            //       });

            //    await policy.ExecuteAsync(() =>
            //    {
            //        return next(context);
            //    });
            //}
            //catch (System.Exception ex)
            //{
            //    if (_rollbackBeforeRetry)
            //    {
            //        await DbContext.RollbackAsync(); //异常后回滚事务

            //    }
            //    if (_throwException)
            //    {
            //        throw;
            //    }
            //    else
            //    {
            //        Logger.LogError(ex, $"Handle Event [{context.GetType()}] Error. The Final Error Message is {ex.ToString()}, retryCount:{_retryCount}");

            //    }
            //}

            //这个抛异常会产生问题，这个要等作者修复。
            //https://github.com/dotnetcore/AspectCore-Framework/blob/master/extras/sample/AspectCore.Extensions.Autofac.WebSample/Startup.cs
            await next(context).ContinueWith(async task =>
            {
                if (task.IsCompleted && !task.IsFaulted)
                {
                    Logger.LogInformation("success");

                }
                else
                {
                    Logger.LogError(task.Exception, $"Handle Event [{context.GetType()}] Error. The Final Error Message is {task.Exception.ToString()}, retryCount:{_retryCount}");

                }
            });
        }
    }
}
