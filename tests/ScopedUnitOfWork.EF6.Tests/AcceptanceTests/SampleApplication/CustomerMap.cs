using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ScopedUnitOfWork.Tests.AcceptanceTests.SampleApplication.SimpleDomain;

namespace ScopedUnitOfWork.EF6.Tests.AcceptanceTests.SampleApplication
{
    public class CustomerMap : EntityTypeConfiguration<Customer>
    {
        public CustomerMap()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).IsRequired();
        }
    }
}