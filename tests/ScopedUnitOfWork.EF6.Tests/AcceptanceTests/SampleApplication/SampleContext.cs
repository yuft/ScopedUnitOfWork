using System.Data.Entity;

namespace ScopedUnitOfWork.EF6.Tests.AcceptanceTests.SampleApplication
{
    public class SampleContext : DbContext
    {
        public SampleContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new CustomerMap());
        }
    }
}