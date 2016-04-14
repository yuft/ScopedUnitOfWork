using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using ScopedUnitOfWork.Tests.AcceptanceTests.SampleApplication.SimpleDomain;

namespace ScopedUnitOfWork.EF.Core.Tests.AcceptanceTests.SampleApplication
{
    public class SampleContext : DbContext
    {
        public SampleContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                .Property(x => x.Name).IsRequired();
        }
    }
}