using Autofac;
using NUnit.Framework;
using ScopedUnitOfWork.Tests.AcceptanceTests;

namespace ScopedUnitOfWork.EF6.Tests.AcceptanceTests
{
    [TestFixture]
    public class UnitOfWorkUsage : UnitOfWorkUsageTestsBase
    {
        protected override IContainer Container => TestsConfiguration.Container;
    }
}