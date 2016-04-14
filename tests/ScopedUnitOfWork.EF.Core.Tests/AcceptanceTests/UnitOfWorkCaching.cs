using Autofac;
using NUnit.Framework;
using ScopedUnitOfWork.Tests.AcceptanceTests;

namespace ScopedUnitOfWork.EF.Core.Tests.AcceptanceTests
{
    [TestFixture]
    public class UnitOfWorkCaching : UnitOfWorkCachingTestsBase
    {
        protected override IContainer Container => TestsConfiguration.Container;
    }
}