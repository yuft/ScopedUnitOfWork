using System;
using FluentAssertions;
using NUnit.Framework;
using ScopedUnitOfWork.Interfaces;
using ScopedUnitOfWork.Tests.AcceptanceTests.SampleApplication.SimpleDomain;
using Customer = ScopedUnitOfWork.Tests.AcceptanceTests.SampleApplication.SimpleDomain.Customer;

namespace ScopedUnitOfWork.Tests.AcceptanceTests
{
    [TestFixture]
    public abstract class UnitOfWorkTransactionsTestsBase : AcceptanceTestBase
    {
        [Test]
        public void Commit_ShouldPersist_OnlyWhenParentTransactionalAndTopMostUnitOfWorkCommits()
        {
            // Arrange
            var customer = CustomerGenerator.Generate();

            using (var uowOutter = GetFactory().Create(ScopeType.Transactional))
            {
                using (var uowMiddle = GetFactory().Create(ScopeType.Transactional))
                {
                    using (var uowInner = GetFactory().Create())
                    {
                        uowInner.GetRepository<ICustomerRepository>().Add(customer);
                        uowInner.Commit();

                        // shows that transactionally changes are not yet saved (note: we are talking about
                        // read commited transaction level which EF uses per default).
                        CheckCustomerExistence(false, customer.Name);
                    }

                    uowMiddle.Commit();

                    // even though a transactional uow commits, it does not mean that changes should be saved
                    // since there is another outer transactional uow
                    CheckCustomerExistence(false, customer.Name);
                }

                // Act
                uowOutter.Commit();
            }

            // Assert
            CheckCustomerExistence(true, customer.Name);
        }

        [Test]
        public void Dispose_ShouldRollbackEverything()
        {
            // Arrange
            var customer = CustomerGenerator.Generate();

            using (GetFactory().Create(ScopeType.Transactional))
            {
                using (var uowMiddle = GetFactory().Create(ScopeType.Transactional))
                {
                    using (var uowInner = GetFactory().Create())
                    {
                        uowInner.GetRepository<ICustomerRepository>().Add(customer);
                        uowInner.Commit();
                    }

                    uowMiddle.Commit();
                }

                // Act - no commit called means dispose will do its thing at the end
            }

            // Assert
            CheckCustomerExistence(false, customer.Name);
        }

        [Test]
        public void MultipleRollBacksShouldBeFine()
        {
            // Arrange
            using (var uow = GetFactory().Create(ScopeType.Transactional))
            {
                uow.GetRepository<ICustomerRepository>().Add(CustomerGenerator.Generate());

                // Act
                uow.Dispose();

                // Assert
                Action action = () =>
                    {
                        uow.Dispose();
                        uow.Dispose();
                    };

                action.ShouldNotThrow();
            }
        }

        [Test]
        public void UsingTransactionsWithMultipleUoWsWhenTransactionalUoWIsUsedForQueryingShouldWork()
        {
            ScopedUnitOfWorkConfiguration.ConfigureDiagnosticsMode(Console.WriteLine, true);

            using (var unitOfWork = GetFactory().Create(ScopeType.Transactional))
            {
                var customer = unitOfWork.GetRepository<ICustomerRepository>().FindByName("name");

                // it does not really matter if customer is null, this is just for a simple query
                if (customer == null)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        using (var uow = GetFactory().Create())
                        {
                            uow.GetRepository<ICustomerRepository>().Add(CustomerGenerator.Generate());
                            uow.Commit();
                        }
                    }
                }

                unitOfWork.Commit();
            }
        }

        private void CheckCustomerExistence(bool shouldExist, string customerName)
        {
            var customer = GetCustomerFromContextDirectly(customerName);

            if (shouldExist)
                customer.Should().NotBeNull();
            else
                customer.Should().BeNull();
        }

        protected abstract Customer GetCustomerFromContextDirectly(string customerName);
    }
}