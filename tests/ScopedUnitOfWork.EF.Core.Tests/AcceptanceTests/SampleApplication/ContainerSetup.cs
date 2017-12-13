using Autofac;
using Microsoft.EntityFrameworkCore;
using ScopedUnitOfWork.Interfaces;
using ScopedUnitOfWork.Tests.AcceptanceTests.SampleApplication.Infrastructure;
using ScopedUnitOfWork.Tests.AcceptanceTests.SampleApplication.SimpleDomain;

namespace ScopedUnitOfWork.EF.Core.Tests.AcceptanceTests.SampleApplication
{
    public class ContainerSetup : ContainerSetupBase
    {
        protected override void RegisterStuff(ContainerBuilder builder)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SampleContext>();
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\MSSQLLocalDB;Database=CoffeeApplied.PersistenceFramework.EF7Tests;Trusted_Connection=True;MultipleActiveResultSets=true");

            builder.Register(x => new SampleContext(optionsBuilder.Options))
                .As<SampleContext>()
                .InstancePerDependency();

            builder.RegisterType<UnitOfWorkFactory<SampleContext>>().As<IUnitOfWorkFactory>();

            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>();
        }
    }
}