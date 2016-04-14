using Autofac;
using NUnit.Framework;
using ScopedUnitOfWork.Interfaces;

namespace ScopedUnitOfWork.Tests.AcceptanceTests
{
    [TestFixture]
    public abstract class AcceptanceTestBase
    {
        protected abstract IContainer Container { get; }

        protected IUnitOfWorkFactory GetFactory()
        {
            return Container.Resolve<IUnitOfWorkFactory>();
        }
    }
}