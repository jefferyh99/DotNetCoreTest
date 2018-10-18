using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoFactTest.Modules;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoFactTest.Dependency
{
    public class AutoFacDependencyResolver : IDependencyResolver
    {

        private readonly IContainer _container;
        public IContainer Container
        {
            get
            {
                return _container;
            }
        }

        public AutoFacDependencyResolver(IServiceCollection services)
        {
            var containerBuilder = new ContainerBuilder();

            //注册Model
            containerBuilder.RegisterModule<DefaultModule>();

            containerBuilder.Populate(services);
            _container = containerBuilder.Build();

        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public T Resolve<T>(Type type)
        {
            object obj = _container.Resolve(type);
            if (obj != null)
                return (T)obj;
            else
                return default(T);
        }

        public T Resolve<T>(string name)
        {
            return _container.ResolveNamed<T>(name);
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> ResolveAll<T>(string name)
        {
            throw new NotImplementedException();
        }
    }
}
