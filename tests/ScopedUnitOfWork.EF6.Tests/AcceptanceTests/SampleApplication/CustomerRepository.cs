using System.Linq;
using ScopedUnitOfWork.Tests.AcceptanceTests.SampleApplication.SimpleDomain;

namespace ScopedUnitOfWork.EF6.Tests.AcceptanceTests.SampleApplication
{
    public class CustomerRepository : GenericRepository<Customer, int>, ICustomerRepository
    {
        public Customer FindByName(string name)
        {
            return Set.SingleOrDefault(x => x.Name == name);
        }
    }
}