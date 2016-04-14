using Autofac;
using NUnit.Framework;
using ScopedUnitOfWork.Tests.AcceptanceTests;

namespace ScopedUnitOfWork.EF.Core.Tests.AcceptanceTests
{
    [TestFixture]
    public class UnitOfWorkUsage : UnitOfWorkUsageTestsBase
    {
        protected override IContainer Container => TestsConfiguration.Container;
    }
}