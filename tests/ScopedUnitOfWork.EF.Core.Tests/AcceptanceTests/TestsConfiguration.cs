using Autofac;
using NUnit.Framework;
using ScopedUnitOfWork.EF.Core.Tests.AcceptanceTests.SampleApplication;

namespace ScopedUnitOfWork.EF.Core.Tests.AcceptanceTests
{
    [SetUpFixture]
    public class TestsConfiguration
    {
        public static IContainer Container { get; private set; }

        [OneTimeSetUp]
        public static void CreateApplication()
        {
            Container = new ContainerSetup().Setup();

            // always make sure we have a fresh database
            using (var context = Container.Resolve<SampleContext>())
            {
                context.Database.EnsureCreated();
            }
        }
    }
}
