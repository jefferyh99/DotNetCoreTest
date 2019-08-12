using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoFactTest.Modules
{
    public class DefaultModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //默认，参数注入，一对一
            builder.RegisterType<Test1>().As<ITest>().InstancePerLifetimeScope();

            //根据名字注入，一对多
            builder.RegisterType<Test1>().Named<ITest>("Test1").InstancePerLifetimeScope();

            //根据名字注入，一对多
            builder.RegisterType<Test2>().Named<ITest>("Test2").InstancePerLifetimeScope();

            //生命周期说明(https://autofaccn.readthedocs.io/en/latest/lifetime/instance-scope.html)
           // builder.RegisterType<Test1>().As<ITest>().InstancePerDependency();//transient，瞬时，每注入都是新的一个
           // builder.RegisterType<Test1>().As<ITest>().SingleInstance();//单例
           // builder.RegisterType<Test1>().As<ITest>().InstancePerLifetimeScope();//每个生命周期内使用同一个
            //using (var scope1 = container.BeginLifetimeScope())
            //{
            //    for (var i = 0; i < 100; i++)
            //    {
            //        // Every time you resolve this from within this
            //        // scope you'll get the same instance.
            //        var w1 = scope1.Resolve<Worker>();
            //    }
            //}

           // builder.RegisterType<Test1>().As<ITest>().InstancePerRequest();//singleton per request，每个http请求一个对象
            //builder.RegisterType<Test1>().As<ITest>().InstancePerLifetimeScope();//Thread Scope


        }
    }
}
