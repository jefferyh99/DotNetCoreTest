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
        }
    }
}
