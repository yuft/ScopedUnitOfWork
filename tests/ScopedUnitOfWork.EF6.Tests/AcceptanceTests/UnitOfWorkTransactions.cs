using System.Linq;
using Autofac;
using NUnit.Framework;
using ScopedUnitOfWork.EF6.Tests.AcceptanceTests.SampleApplication;
using ScopedUnitOfWork.Tests.AcceptanceTests;
using ScopedUnitOfWork.Tests.AcceptanceTests.SampleApplication.SimpleDomain;

namespace ScopedUnitOfWork.EF6.Tests.AcceptanceTests
{
    [TestFixture]
    public class UnitOfWorkTransactions : UnitOfWorkTransactionsTestsBase
    {
        protected override IContainer Container => TestsConfiguration.Container;

        protected override Customer GetCustomerFromContextDirectly(string customerName)
        {
            var context = Container.Resolve<SampleContext>();
            return context.Set<Customer>().SingleOrDefault(x => x.Name == customerName);
        }
    }
}