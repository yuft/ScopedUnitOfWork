using Autofac;
using ScopedUnitOfWork.Interfaces;
using ScopedUnitOfWork.Tests.AcceptanceTests.SampleApplication.Infrastructure;
using ScopedUnitOfWork.Tests.AcceptanceTests.SampleApplication.SimpleDomain;

namespace ScopedUnitOfWork.EF6.Tests.AcceptanceTests.SampleApplication
{
    public class ContainerSetup : ContainerSetupBase
    {
        protected override void RegisterStuff(ContainerBuilder builder)
        {
            builder.Register(x => new SampleContext(
                "Server=(localdb)\\MSSQLLocalDB;Database=ScopedUnitOfWork.EF6Tests;Trusted_Connection=True;MultipleActiveResultSets=true"))
                .As<SampleContext>()
                .InstancePerDependency();

            builder.RegisterType<UnitOfWorkFactory<SampleContext>>().As<IUnitOfWorkFactory>();
            
            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>();
        }
    }
}