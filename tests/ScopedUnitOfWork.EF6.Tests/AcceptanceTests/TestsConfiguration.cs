using System.Data.Entity;
using Autofac;
using NUnit.Framework;
using ScopedUnitOfWork.EF6.Tests.AcceptanceTests.SampleApplication;

namespace ScopedUnitOfWork.EF6.Tests.AcceptanceTests
{
    [SetUpFixture]
    public class TestsConfiguration
    {
        public static IContainer Container { get; private set; }

        [SetUp]
        public static void CreateApplication()
        {
            Container = new ContainerSetup().Setup();

            // always make sure we have a fresh database
            Database.SetInitializer(new DropCreateDatabaseAlways<SampleContext>());
        }
    }
}
