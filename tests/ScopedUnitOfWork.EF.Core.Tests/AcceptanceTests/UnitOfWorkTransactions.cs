using System.Linq;
using Autofac;
using NUnit.Framework;
using ScopedUnitOfWork.EF.Core.Tests.AcceptanceTests.SampleApplication;
using ScopedUnitOfWork.Tests.AcceptanceTests;
using ScopedUnitOfWork.Tests.AcceptanceTests.SampleApplication.SimpleDomain;

namespace ScopedUnitOfWork.EF.Core.Tests.AcceptanceTests
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