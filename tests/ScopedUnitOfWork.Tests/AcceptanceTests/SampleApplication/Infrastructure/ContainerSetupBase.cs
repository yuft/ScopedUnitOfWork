using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Autofac.Features.ResolveAnything;
using CommonServiceLocator;

namespace ScopedUnitOfWork.Tests.AcceptanceTests.SampleApplication.Infrastructure
{
    public abstract class ContainerSetupBase
    {
        public IContainer Setup()
        {
            var builder = new ContainerBuilder();
            
            RegisterStuff(builder);
            
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            var container = builder.Build();

            // update the container with "self" registation (so that no static IoC or 
            // similar interfaces are used)
            var serviceLocator = new AutofacServiceLocator(container);
            builder = new ContainerBuilder();
            builder.RegisterInstance(serviceLocator).As<IServiceLocator>();
            builder.Update(container);

            return container;
        }

        protected abstract void RegisterStuff(ContainerBuilder builder);
    }
}
